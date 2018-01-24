﻿using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using HETSAPI.Models;
using Project = HETSAPI.ImportModels.Project;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Ptoject Records
    /// </summary>
    public static class ImportProject
    {
        const string OldTable = "Project";
        const string NewTable = "HET_PROJECT";
        const string XmlFileName = "Project.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Projects
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Project[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Project[] legacyItems = (Project[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (Project item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Project_Id.ToString());

                    // new entry
                    if (importMap == null) 
                    {
                        if (item.Project_Id > 0)
                        {
                            Models.Project instance = null;
                            CopyToInstance(dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Project_Id.ToString(), NewTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.Project instance = dbContext.Projects.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (instance == null) 
                        {
                            CopyToInstance(dbContext, item, ref instance, systemId);

                            // update the import map
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(dbContext, item, ref instance, systemId);

                            // touch the import map
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BCBidImport.SigId);
                            dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            performContext.WriteLine("Error saving data " + e.Message);
                        }
                    }
                }

                try
                {
                    performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BCBidImport.SigId.ToString(), BCBidImport.SigId);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    performContext.WriteLine("Error saving data " + e.Message);
                }
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, Project oldObject, ref Models.Project instance, string systemId)
        {
            if (oldObject.Project_Id <= 0)
                return;

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new Models.Project {Id = oldObject.Project_Id};

                try
                {
                    try
                    {   //4 properties
                        instance.ProvincialProjectNumber = oldObject.Project_Num;
                        ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == oldObject.Service_Area_Id);
                        District dis = dbContext.Districts.FirstOrDefault(x => x.Id == serviceArea.DistrictId);

                        if (dis != null)   
                        {
                            instance.District = dis;
                            instance.DistrictId = dis.Id;
                        }
                        else   
                        {
                            // this means that the District Id is not in the database
                            // (happens when the production data does not include district Other than "Lower Mainland" or all the districts)
                            return;
                        }
                    }
                    catch
                    {
                        // do nothing
                    }

                    try
                    {
                        instance.Name = oldObject.Job_Desc1;
                    }
                    catch 
                    {
                        // do nothing
                    }

                    try
                    {
                        instance.Information = oldObject.Job_Desc2;
                    }
                    catch
                    {
                        // do nothing
                    }

                    try
                    {
                        instance.Notes = new List<Note>();

                        Note note = new Note
                        {
                            Text = new string(oldObject.Job_Desc2.Take(2048).ToArray()),
                            IsNoLongerRelevant = true
                        };

                        instance.Notes.Add(note);
                    }
                    catch
                    {
                        // do nothing
                    }
                    
                    try
                    {   
                        instance.CreateTimestamp = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        instance.CreateTimestamp = DateTime.UtcNow;
                    }

                    instance.CreateUserid = createdBy.SmUserId;
                }
                catch
                {
                    // do nothing
                }

                dbContext.Projects.Add(instance);
            }
            else
            {
                instance = dbContext.Projects.First(x => x.Id == oldObject.Project_Id);
                instance.LastUpdateUserid = modifiedBy.SmUserId;
                instance.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.Projects.Update(instance);
            }
        }
    }
}

