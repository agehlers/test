using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Renatal Request Attachment Database Model
    /// </summary>
    [MetaData (Description = "Attachments that are required as part of the Rental Requests")]
    public sealed class RentalRequestAttachment : AuditableEntity, IEquatable<RentalRequestAttachment>
    {
        /// <summary>
        /// REntal Request Attachment Database Model Constructor (required by entity framework)
        /// </summary>
        public RentalRequestAttachment()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestAttachment" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a RentalRequestAttachment (required).</param>
        /// <param name="rentalRequest">A foreign key reference to the system-generated unique identifier for a Rental Request (required).</param>
        /// <param name="attachment">The name&amp;#x2F;type attachment needed as part of the fulfillment of the request (required).</param>
        public RentalRequestAttachment(int id, RentalRequest rentalRequest, string attachment)
        {   
            Id = id;
            RentalRequest = rentalRequest;
            Attachment = attachment;
        }

        /// <summary>
        /// A system-generated unique identifier for a RentalRequestAttachment
        /// </summary>
        /// <value>A system-generated unique identifier for a RentalRequestAttachment</value>
        [MetaData (Description = "A system-generated unique identifier for a RentalRequestAttachment")]
        public int Id { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Rental Request
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Rental Request</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Rental Request")]
        public RentalRequest RentalRequest { get; set; }
        
        /// <summary>
        /// Foreign key for RentalRequest 
        /// </summary>   
        [ForeignKey("RentalRequest")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Rental Request")]
        public int? RentalRequestId { get; set; }
        
        /// <summary>
        /// The name&#x2F;type attachment needed as part of the fulfillment of the request
        /// </summary>
        /// <value>The name&#x2F;type attachment needed as part of the fulfillment of the request</value>
        [MetaData (Description = "The name&#x2F;type attachment needed as part of the fulfillment of the request")]
        [MaxLength(150)]        
        public string Attachment { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RentalRequestAttachment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RentalRequest: ").Append(RentalRequest).Append("\n");
            sb.Append("  Attachment: ").Append(Attachment).Append("\n");
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
            return obj.GetType() == GetType() && Equals((RentalRequestAttachment)obj);
        }

        /// <summary>
        /// Returns true if RentalRequestAttachment instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequestAttachment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestAttachment other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    RentalRequest == other.RentalRequest ||
                    RentalRequest != null &&
                    RentalRequest.Equals(other.RentalRequest)
                ) &&                 
                (
                    Attachment == other.Attachment ||
                    Attachment != null &&
                    Attachment.Equals(other.Attachment)
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
                
                if (RentalRequest != null)
                {
                    hash = hash * 59 + RentalRequest.GetHashCode();
                }

                if (Attachment != null)
                {
                    hash = hash * 59 + Attachment.GetHashCode();
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
        public static bool operator ==(RentalRequestAttachment left, RentalRequestAttachment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequestAttachment left, RentalRequestAttachment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
