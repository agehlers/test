using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Equipment Database Model
    /// </summary>
    [MetaData (Description = "A piece of equipment in the HETS system. Each piece of equipment is of a specific equipment type, owned by an Owner, and is within a Local Area.")]
    public sealed partial class Equipment : AuditableEntity, IEquatable<Equipment>
    {
        /// <summary>
        /// Equipment Database Model Constructor (required by entity framework)
        /// </summary>
        public Equipment()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Equipment (required).</param>
        /// <param name="localArea">A foreign key reference to the system-generated unique identifier for a Local Area (required).</param>
        /// <param name="districtEquipmentType">A foreign key reference to the system-generated unique identifier for a Equipment Type (required).</param>
        /// <param name="owner">A foreign key reference to the system-generated unique identifier for an Owner (required).</param>
        /// <param name="equipmentCode">A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083. (required).</param>
        /// <param name="status">The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added. (required).</param>
        /// <param name="receivedDate">The date the piece of equipment was first received and recorded in HETS. (required).</param>
        /// <param name="lastVerifiedDate">The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme. (required).</param>
        /// <param name="approvedDate">The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date..</param>
        /// <param name="isInformationUpdateNeeded">Set true if a need to update the information&amp;#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update..</param>
        /// <param name="informationUpdateNeededReason">A note about why the needed information&amp;#x2F;status update that is needed about the equipment..</param>
        /// <param name="licencePlate">The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk..</param>
        /// <param name="make">The make of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="model">The model of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="year">The model year of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="operator">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="payRate">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="refuseRate">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="serialNumber">The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area..</param>
        /// <param name="size">The size of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="toDate">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="blockNumber">The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open.</param>
        /// <param name="seniority">The current seniority calculation result for this piece of equipment. The calculation is based on the &amp;quot;numYears&amp;quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo)..</param>
        /// <param name="numberInBlock">The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order..</param>
        /// <param name="isSeniorityOverridden">True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden..</param>
        /// <param name="seniorityOverrideReason">A text reason for why the piece of equipments underlying data was overridden to change their seniority number..</param>
        /// <param name="seniorityEffectiveDate">The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated..</param>
        /// <param name="yearsOfService">The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY..</param>
        /// <param name="serviceHoursLastYear">Number of hours of service by this piece of equipment in the previous fiscal year.</param>
        /// <param name="serviceHoursTwoYearsAgo">Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016.</param>
        /// <param name="serviceHoursThreeYearsAgo">Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015.</param>
        /// <param name="archiveCode">TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived..</param>
        /// <param name="archiveReason">An optional comment about why this piece of equipment has been archived..</param>
        /// <param name="archiveDate">The date on which a user most recenly marked this piece of equipment as archived..</param>
        /// <param name="dumpTruck">A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck..</param>
        /// <param name="equipmentAttachments">EquipmentAttachments.</param>
        /// <param name="notes">Notes.</param>
        /// <param name="attachments">Attachments.</param>
        /// <param name="history">History.</param>
        public Equipment(int id, LocalArea localArea, DistrictEquipmentType districtEquipmentType, Owner owner, string equipmentCode, 
            string status, DateTime receivedDate, DateTime lastVerifiedDate, DateTime? approvedDate = null, 
            bool? isInformationUpdateNeeded = null, string informationUpdateNeededReason = null, string licencePlate = null, 
            string make = null, string model = null, string year = null, string @operator = null, float? payRate = null, 
            string refuseRate = null, string serialNumber = null, string size = null, DateTime? toDate = null, int? blockNumber = null, 
            float? seniority = null, int? numberInBlock = null, bool? isSeniorityOverridden = null, string seniorityOverrideReason = null, 
            DateTime? seniorityEffectiveDate = null, float? yearsOfService = null, float? serviceHoursLastYear = null, 
            float? serviceHoursTwoYearsAgo = null, float? serviceHoursThreeYearsAgo = null, string archiveCode = null, 
            string archiveReason = null, DateTime? archiveDate = null, DumpTruck dumpTruck = null, 
            List<EquipmentAttachment> equipmentAttachments = null, List<Note> notes = null, List<Attachment> attachments = null, 
            List<History> history = null)
        {
            Id = id;
            LocalArea = localArea;
            DistrictEquipmentType = districtEquipmentType;
            Owner = owner;
            EquipmentCode = equipmentCode;
            Status = status;
            ReceivedDate = receivedDate;
            LastVerifiedDate = lastVerifiedDate;            
            ApprovedDate = approvedDate;
            IsInformationUpdateNeeded = isInformationUpdateNeeded;
            InformationUpdateNeededReason = informationUpdateNeededReason;
            LicencePlate = licencePlate;
            Make = make;
            Model = model;
            Year = year;
            Operator = @operator;
            PayRate = payRate;
            RefuseRate = refuseRate;
            SerialNumber = serialNumber;
            Size = size;
            ToDate = toDate;
            BlockNumber = blockNumber;
            Seniority = seniority;
            NumberInBlock = numberInBlock;
            IsSeniorityOverridden = isSeniorityOverridden;
            SeniorityOverrideReason = seniorityOverrideReason;
            SeniorityEffectiveDate = seniorityEffectiveDate;
            YearsOfService = yearsOfService;
            ServiceHoursLastYear = serviceHoursLastYear;
            ServiceHoursTwoYearsAgo = serviceHoursTwoYearsAgo;
            ServiceHoursThreeYearsAgo = serviceHoursThreeYearsAgo;
            ArchiveCode = archiveCode;
            ArchiveReason = archiveReason;
            ArchiveDate = archiveDate;
            DumpTruck = dumpTruck;
            EquipmentAttachments = equipmentAttachments;
            Notes = notes;
            Attachments = attachments;
            History = history;
        }

        /// <summary>
        /// A system-generated unique identifier for a Equipment
        /// </summary>
        /// <value>A system-generated unique identifier for a Equipment</value>
        [MetaData (Description = "A system-generated unique identifier for a Equipment")]
        public int Id { get; set; }

        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Local Area
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Local Area</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public LocalArea LocalArea { get; set; }

        /// <summary>
        /// Foreign key for LocalArea
        /// </summary>
        [ForeignKey("LocalArea")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public int? LocalAreaId { get; set; }

        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Equipment Type
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Equipment Type</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Equipment Type")]
        public DistrictEquipmentType DistrictEquipmentType { get; set; }

        /// <summary>
        /// Foreign key for DistrictEquipmentType
        /// </summary>
        [ForeignKey("DistrictEquipmentType")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Equipment Type")]
        public int? DistrictEquipmentTypeId { get; set; }

        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Owner
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Owner</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Owner")]
        public Owner Owner { get; set; }

        /// <summary>
        /// Foreign key for Owner
        /// </summary>
        [ForeignKey("Owner")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Owner")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.
        /// </summary>
        /// <value>A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.</value>
        [MetaData (Description = "A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.")]
        [MaxLength(25)]

        public string EquipmentCode { get; set; }

        /// <summary>
        /// The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.
        /// </summary>
        /// <value>The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.</value>
        [MetaData (Description = "The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.")]
        [MaxLength(50)]

        public string Status { get; set; }

        /// <summary>
        /// The date the piece of equipment was first received and recorded in HETS.
        /// </summary>
        /// <value>The date the piece of equipment was first received and recorded in HETS.</value>
        [MetaData (Description = "The date the piece of equipment was first received and recorded in HETS.")]
        public DateTime ReceivedDate { get; set; }

        /// <summary>
        /// The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.
        /// </summary>
        /// <value>The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.</value>
        [MetaData (Description = "The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.")]
        public DateTime LastVerifiedDate { get; set; }

        /// <summary>
        /// The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.
        /// </summary>
        /// <value>The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.</value>
        [MetaData (Description = "The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.")]
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.
        /// </summary>
        /// <value>Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.</value>
        [MetaData (Description = "Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.")]
        public bool? IsInformationUpdateNeeded { get; set; }

        /// <summary>
        /// A note about why the needed information&#x2F;status update that is needed about the equipment.
        /// </summary>
        /// <value>A note about why the needed information&#x2F;status update that is needed about the equipment.</value>
        [MetaData (Description = "A note about why the needed information&#x2F;status update that is needed about the equipment.")]
        [MaxLength(2048)]

        public string InformationUpdateNeededReason { get; set; }

        /// <summary>
        /// The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.
        /// </summary>
        /// <value>The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.</value>
        [MetaData (Description = "The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.")]
        [MaxLength(20)]

        public string LicencePlate { get; set; }

        /// <summary>
        /// The make of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The make of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaData (Description = "The make of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(50)]

        public string Make { get; set; }

        /// <summary>
        /// The model of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The model of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaData (Description = "The model of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(50)]

        public string Model { get; set; }

        /// <summary>
        /// The model year of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The model year of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaData (Description = "The model year of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(15)]

        public string Year { get; set; }

        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaData (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        [MaxLength(255)]

        public string Operator { get; set; }

        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaData (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? PayRate { get; set; }

        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaData (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        [MaxLength(255)]

        public string RefuseRate { get; set; }

        /// <summary>
        /// The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.
        /// </summary>
        /// <value>The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.</value>
        [MetaData (Description = "The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.")]
        [MaxLength(100)]

        public string SerialNumber { get; set; }

        /// <summary>
        /// The size of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The size of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaData (Description = "The size of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(128)]

        public string Size { get; set; }

        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaData (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open
        /// </summary>
        /// <value>The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open</value>
        [MetaData (Description = "The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open")]
        public int? BlockNumber { get; set; }

        /// <summary>
        /// The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).
        /// </summary>
        /// <value>The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).</value>
        [MetaData (Description = "The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).")]
        public float? Seniority { get; set; }

        /// <summary>
        /// The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.
        /// </summary>
        /// <value>The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.</value>
        [MetaData (Description = "The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.")]
        public int? NumberInBlock { get; set; }

        /// <summary>
        /// True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.
        /// </summary>
        /// <value>True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.</value>
        [MetaData (Description = "True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.")]
        public bool? IsSeniorityOverridden { get; set; }

        /// <summary>
        /// A text reason for why the piece of equipments underlying data was overridden to change their seniority number.
        /// </summary>
        /// <value>A text reason for why the piece of equipments underlying data was overridden to change their seniority number.</value>
        [MetaData (Description = "A text reason for why the piece of equipments underlying data was overridden to change their seniority number.")]
        [MaxLength(2048)]

        public string SeniorityOverrideReason { get; set; }

        /// <summary>
        /// The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.
        /// </summary>
        /// <value>The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.</value>
        [MetaData (Description = "The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.")]
        public DateTime? SeniorityEffectiveDate { get; set; }

        /// <summary>
        /// The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.
        /// </summary>
        /// <value>The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.</value>
        [MetaData (Description = "The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.")]
        public float? YearsOfService { get; set; }

        /// <summary>
        /// Number of hours of service by this piece of equipment in the previous fiscal year
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the previous fiscal year</value>
        [MetaData (Description = "Number of hours of service by this piece of equipment in the previous fiscal year")]
        public float? ServiceHoursLastYear { get; set; }

        /// <summary>
        /// Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016</value>
        [MetaData (Description = "Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016")]
        public float? ServiceHoursTwoYearsAgo { get; set; }

        /// <summary>
        /// Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015</value>
        [MetaData (Description = "Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015")]
        public float? ServiceHoursThreeYearsAgo { get; set; }

        /// <summary>
        /// TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived.
        /// </summary>
        /// <value>TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived.</value>
        [MetaData (Description = "TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived.")]
        [MaxLength(50)]

        public string ArchiveCode { get; set; }

        /// <summary>
        /// An optional comment about why this piece of equipment has been archived.
        /// </summary>
        /// <value>An optional comment about why this piece of equipment has been archived.</value>
        [MetaData (Description = "An optional comment about why this piece of equipment has been archived.")]
        [MaxLength(2048)]

        public string ArchiveReason { get; set; }

        /// <summary>
        /// The date on which a user most recenly marked this piece of equipment as archived.
        /// </summary>
        /// <value>The date on which a user most recenly marked this piece of equipment as archived.</value>
        [MetaData (Description = "The date on which a user most recenly marked this piece of equipment as archived.")]
        public DateTime? ArchiveDate { get; set; }

        /// <summary>
        /// A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.
        /// </summary>
        /// <value>A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.</value>
        [MetaData (Description = "A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.")]
        public DumpTruck DumpTruck { get; set; }

        /// <summary>
        /// Foreign key for DumpTruck
        /// </summary>
        [ForeignKey("DumpTruck")]
		[JsonIgnore]
		[MetaData (Description = "A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.")]
        public int? DumpTruckId { get; set; }

        /// <summary>
        /// Gets or Sets EquipmentAttachments
        /// </summary>
        public List<EquipmentAttachment> EquipmentAttachments { get; set; }

        /// <summary>
        /// Gets or Sets Notes
        /// </summary>
        public List<Note> Notes { get; set; }

        /// <summary>
        /// Gets or Sets Attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Gets or Sets History
        /// </summary>
        public List<History> History { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Equipment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  DistrictEquipmentType: ").Append(DistrictEquipmentType).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  EquipmentCode: ").Append(EquipmentCode).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ReceivedDate: ").Append(ReceivedDate).Append("\n");
            sb.Append("  LastVerifiedDate: ").Append(LastVerifiedDate).Append("\n");
            sb.Append("  ApprovedDate: ").Append(ApprovedDate).Append("\n");
            sb.Append("  IsInformationUpdateNeeded: ").Append(IsInformationUpdateNeeded).Append("\n");
            sb.Append("  InformationUpdateNeededReason: ").Append(InformationUpdateNeededReason).Append("\n");
            sb.Append("  LicencePlate: ").Append(LicencePlate).Append("\n");
            sb.Append("  Make: ").Append(Make).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("  Year: ").Append(Year).Append("\n");
            sb.Append("  Operator: ").Append(Operator).Append("\n");
            sb.Append("  PayRate: ").Append(PayRate).Append("\n");
            sb.Append("  RefuseRate: ").Append(RefuseRate).Append("\n");
            sb.Append("  SerialNumber: ").Append(SerialNumber).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  ToDate: ").Append(ToDate).Append("\n");
            sb.Append("  BlockNumber: ").Append(BlockNumber).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  NumberInBlock: ").Append(NumberInBlock).Append("\n");
            sb.Append("  IsSeniorityOverridden: ").Append(IsSeniorityOverridden).Append("\n");
            sb.Append("  SeniorityOverrideReason: ").Append(SeniorityOverrideReason).Append("\n");
            sb.Append("  SeniorityEffectiveDate: ").Append(SeniorityEffectiveDate).Append("\n");
            sb.Append("  YearsOfService: ").Append(YearsOfService).Append("\n");
            sb.Append("  ServiceHoursLastYear: ").Append(ServiceHoursLastYear).Append("\n");
            sb.Append("  ServiceHoursTwoYearsAgo: ").Append(ServiceHoursTwoYearsAgo).Append("\n");
            sb.Append("  ServiceHoursThreeYearsAgo: ").Append(ServiceHoursThreeYearsAgo).Append("\n");
            sb.Append("  ArchiveCode: ").Append(ArchiveCode).Append("\n");
            sb.Append("  ArchiveReason: ").Append(ArchiveReason).Append("\n");
            sb.Append("  ArchiveDate: ").Append(ArchiveDate).Append("\n");
            sb.Append("  DumpTruck: ").Append(DumpTruck).Append("\n");
            sb.Append("  EquipmentAttachments: ").Append(EquipmentAttachments).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((Equipment)obj);
        }

        /// <summary>
        /// Returns true if Equipment instances are equal
        /// </summary>
        /// <param name="other">Instance of Equipment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Equipment other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&
                (
                    LocalArea == other.LocalArea ||
                    LocalArea != null &&
                    LocalArea.Equals(other.LocalArea)
                ) &&
                (
                    DistrictEquipmentType == other.DistrictEquipmentType ||
                    DistrictEquipmentType != null &&
                    DistrictEquipmentType.Equals(other.DistrictEquipmentType)
                ) &&
                (
                    Owner == other.Owner ||
                    Owner != null &&
                    Owner.Equals(other.Owner)
                ) &&
                (
                    EquipmentCode == other.EquipmentCode ||
                    EquipmentCode != null &&
                    EquipmentCode.Equals(other.EquipmentCode)
                ) &&
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) &&
                (
                    ReceivedDate == other.ReceivedDate ||
                    ReceivedDate.Equals(other.ReceivedDate)
                ) &&
                (
                    LastVerifiedDate == other.LastVerifiedDate ||
                    LastVerifiedDate.Equals(other.LastVerifiedDate)
                ) &&
                (
                    ApprovedDate == other.ApprovedDate ||
                    ApprovedDate != null &&
                    ApprovedDate.Equals(other.ApprovedDate)
                ) &&
                (
                    IsInformationUpdateNeeded == other.IsInformationUpdateNeeded ||
                    IsInformationUpdateNeeded != null &&
                    IsInformationUpdateNeeded.Equals(other.IsInformationUpdateNeeded)
                ) &&
                (
                    InformationUpdateNeededReason == other.InformationUpdateNeededReason ||
                    InformationUpdateNeededReason != null &&
                    InformationUpdateNeededReason.Equals(other.InformationUpdateNeededReason)
                ) &&
                (
                    LicencePlate == other.LicencePlate ||
                    LicencePlate != null &&
                    LicencePlate.Equals(other.LicencePlate)
                ) &&
                (
                    Make == other.Make ||
                    Make != null &&
                    Make.Equals(other.Make)
                ) &&
                (
                    Model == other.Model ||
                    Model != null &&
                    Model.Equals(other.Model)
                ) &&
                (
                    Year == other.Year ||
                    Year != null &&
                    Year.Equals(other.Year)
                ) &&
                (
                    Operator == other.Operator ||
                    Operator != null &&
                    Operator.Equals(other.Operator)
                ) &&
                (
                    PayRate == other.PayRate ||
                    PayRate != null &&
                    PayRate.Equals(other.PayRate)
                ) &&
                (
                    RefuseRate == other.RefuseRate ||
                    RefuseRate != null &&
                    RefuseRate.Equals(other.RefuseRate)
                ) &&
                (
                    SerialNumber == other.SerialNumber ||
                    SerialNumber != null &&
                    SerialNumber.Equals(other.SerialNumber)
                ) &&
                (
                    Size == other.Size ||
                    Size != null &&
                    Size.Equals(other.Size)
                ) &&
                (
                    ToDate == other.ToDate ||
                    ToDate != null &&
                    ToDate.Equals(other.ToDate)
                ) &&
                (
                    BlockNumber == other.BlockNumber ||
                    BlockNumber != null &&
                    BlockNumber.Equals(other.BlockNumber)
                ) &&
                (
                    Seniority == other.Seniority ||
                    Seniority != null &&
                    Seniority.Equals(other.Seniority)
                ) &&
                (
                    NumberInBlock == other.NumberInBlock ||
                    NumberInBlock != null &&
                    NumberInBlock.Equals(other.NumberInBlock)
                ) &&
                (
                    IsSeniorityOverridden == other.IsSeniorityOverridden ||
                    IsSeniorityOverridden != null &&
                    IsSeniorityOverridden.Equals(other.IsSeniorityOverridden)
                ) &&
                (
                    SeniorityOverrideReason == other.SeniorityOverrideReason ||
                    SeniorityOverrideReason != null &&
                    SeniorityOverrideReason.Equals(other.SeniorityOverrideReason)
                ) &&
                (
                    SeniorityEffectiveDate == other.SeniorityEffectiveDate ||
                    SeniorityEffectiveDate != null &&
                    SeniorityEffectiveDate.Equals(other.SeniorityEffectiveDate)
                ) &&
                (
                    YearsOfService == other.YearsOfService ||
                    YearsOfService != null &&
                    YearsOfService.Equals(other.YearsOfService)
                ) &&
                (
                    ServiceHoursLastYear == other.ServiceHoursLastYear ||
                    ServiceHoursLastYear != null &&
                    ServiceHoursLastYear.Equals(other.ServiceHoursLastYear)
                ) &&
                (
                    ServiceHoursTwoYearsAgo == other.ServiceHoursTwoYearsAgo ||
                    ServiceHoursTwoYearsAgo != null &&
                    ServiceHoursTwoYearsAgo.Equals(other.ServiceHoursTwoYearsAgo)
                ) &&
                (
                    ServiceHoursThreeYearsAgo == other.ServiceHoursThreeYearsAgo ||
                    ServiceHoursThreeYearsAgo != null &&
                    ServiceHoursThreeYearsAgo.Equals(other.ServiceHoursThreeYearsAgo)
                ) &&
                (
                    ArchiveCode == other.ArchiveCode ||
                    ArchiveCode != null &&
                    ArchiveCode.Equals(other.ArchiveCode)
                ) &&
                (
                    ArchiveReason == other.ArchiveReason ||
                    ArchiveReason != null &&
                    ArchiveReason.Equals(other.ArchiveReason)
                ) &&
                (
                    ArchiveDate == other.ArchiveDate ||
                    ArchiveDate != null &&
                    ArchiveDate.Equals(other.ArchiveDate)
                ) &&
                (
                    DumpTruck == other.DumpTruck ||
                    DumpTruck != null &&
                    DumpTruck.Equals(other.DumpTruck)
                ) &&
                (
                    EquipmentAttachments == other.EquipmentAttachments ||
                    EquipmentAttachments != null &&
                    EquipmentAttachments.SequenceEqual(other.EquipmentAttachments)
                ) &&
                (
                    Notes == other.Notes ||
                    Notes != null &&
                    Notes.SequenceEqual(other.Notes)
                ) &&
                (
                    Attachments == other.Attachments ||
                    Attachments != null &&
                    Attachments.SequenceEqual(other.Attachments)
                ) &&
                (
                    History == other.History ||
                    History != null &&
                    History.SequenceEqual(other.History)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;

                // Suitable nullity checks
                hash = hash * 59 + Id.GetHashCode();

                if (LocalArea != null)
                {
                    hash = hash * 59 + LocalArea.GetHashCode();
                }

                if (DistrictEquipmentType != null)
                {
                    hash = hash * 59 + DistrictEquipmentType.GetHashCode();
                }

                if (Owner != null)
                {
                    hash = hash * 59 + Owner.GetHashCode();
                }

                if (EquipmentCode != null)
                {
                    hash = hash * 59 + EquipmentCode.GetHashCode();
                }                

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
                }

                hash = hash * 59 + ReceivedDate.GetHashCode();
                hash = hash * 59 + LastVerifiedDate.GetHashCode();                

                if (ApprovedDate != null)
                {
                    hash = hash * 59 + ApprovedDate.GetHashCode();
                }

                if (IsInformationUpdateNeeded != null)
                {
                    hash = hash * 59 + IsInformationUpdateNeeded.GetHashCode();
                }

                if (InformationUpdateNeededReason != null)
                {
                    hash = hash * 59 + InformationUpdateNeededReason.GetHashCode();
                }

                if (LicencePlate != null)
                {
                    hash = hash * 59 + LicencePlate.GetHashCode();
                }

                if (Make != null)
                {
                    hash = hash * 59 + Make.GetHashCode();
                }

                if (Model != null)
                {
                    hash = hash * 59 + Model.GetHashCode();
                }

                if (Year != null)
                {
                    hash = hash * 59 + Year.GetHashCode();
                }

                if (Operator != null)
                {
                    hash = hash * 59 + Operator.GetHashCode();
                }

                if (PayRate != null)
                {
                    hash = hash * 59 + PayRate.GetHashCode();
                }

                if (RefuseRate != null)
                {
                    hash = hash * 59 + RefuseRate.GetHashCode();
                }

                if (SerialNumber != null)
                {
                    hash = hash * 59 + SerialNumber.GetHashCode();
                }

                if (Size != null)
                {
                    hash = hash * 59 + Size.GetHashCode();
                }

                if (ToDate != null)
                {
                    hash = hash * 59 + ToDate.GetHashCode();
                }

                if (BlockNumber != null)
                {
                    hash = hash * 59 + BlockNumber.GetHashCode();
                }

                if (Seniority != null)
                {
                    hash = hash * 59 + Seniority.GetHashCode();
                }

                if (NumberInBlock != null)
                {
                    hash = hash * 59 + NumberInBlock.GetHashCode();
                }

                if (IsSeniorityOverridden != null)
                {
                    hash = hash * 59 + IsSeniorityOverridden.GetHashCode();
                }

                if (SeniorityOverrideReason != null)
                {
                    hash = hash * 59 + SeniorityOverrideReason.GetHashCode();
                }

                if (SeniorityEffectiveDate != null)
                {
                    hash = hash * 59 + SeniorityEffectiveDate.GetHashCode();
                }

                if (YearsOfService != null)
                {
                    hash = hash * 59 + YearsOfService.GetHashCode();
                }

                if (ServiceHoursLastYear != null)
                {
                    hash = hash * 59 + ServiceHoursLastYear.GetHashCode();
                }

                if (ServiceHoursTwoYearsAgo != null)
                {
                    hash = hash * 59 + ServiceHoursTwoYearsAgo.GetHashCode();
                }

                if (ServiceHoursThreeYearsAgo != null)
                {
                    hash = hash * 59 + ServiceHoursThreeYearsAgo.GetHashCode();
                }

                if (ArchiveCode != null)
                {
                    hash = hash * 59 + ArchiveCode.GetHashCode();
                }

                if (ArchiveReason != null)
                {
                    hash = hash * 59 + ArchiveReason.GetHashCode();
                }

                if (ArchiveDate != null)
                {
                    hash = hash * 59 + ArchiveDate.GetHashCode();
                }

                if (DumpTruck != null)
                {
                    hash = hash * 59 + DumpTruck.GetHashCode();
                }

                if (EquipmentAttachments != null)
                {
                    hash = hash * 59 + EquipmentAttachments.GetHashCode();
                }

                if (Notes != null)
                {
                    hash = hash * 59 + Notes.GetHashCode();
                }

                if (Attachments != null)
                {
                    hash = hash * 59 + Attachments.GetHashCode();
                }

                if (History != null)
                {
                    hash = hash * 59 + History.GetHashCode();
                }

                return hash;
            }
        }

        #region Operators

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Equipment left, Equipment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Equipment left, Equipment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
