﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HETSAPI.Models;

namespace HETSAPI.Seeders
{
    public class RegionSeeder : Seeder<DbAppContext>
    {
        private readonly string[] ProfileTriggers = { AllProfiles };

        public RegionSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles
        {
            get { return ProfileTriggers; }
        }

        protected override void Invoke(DbAppContext context)
        {
            UpdateRegions(context);
            context.SaveChanges();
        }

        private void UpdateRegions(DbAppContext context)
        {
            List<Region> seedRegions = GetSeedRegions();

            foreach (Region region in seedRegions)
            {
                context.UpdateSeedRegionInfo(region);
                context.SaveChanges();
            }

            AddInitialRegions(context);            
        }

        private void AddInitialRegions(DbAppContext context)
        {
            context.AddInitialRegionsFromFile(Configuration["RegionInitializationFile"]);
        }

        private List<Region> GetSeedRegions()
        {
            List<Region> regions = new List<Region>(GetDefaultRegions());

            if (IsDevelopmentEnvironment)
                regions.AddRange(GetDevRegions());

            if (IsTestEnvironment || IsStagingEnvironment)
                regions.AddRange(GetTestRegions());

            if (IsProductionEnvironment)
                regions.AddRange(GetProdRegions());

            return regions;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<Region> GetDefaultRegions()
        {
            return new List<Region>();
        }

        /// <summary>
        /// Returns a list of regions to be populated in the Development environment.
        /// </summary>
        private List<Region> GetDevRegions()
        {
            return new List<Region>();            
        }

        /// <summary>
        /// Returns a list of regions to be populated in the Test environment.
        /// </summary>
        private List<Region> GetTestRegions()
        {
            return new List<Region>();
        }

        /// <summary>
        /// Returns a list of regions to be populated in the Production environment.
        /// </summary>
        private List<Region> GetProdRegions()
        {
            return new List<Region>();
        }
    }
}
