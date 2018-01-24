using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Permission View Model
    /// </summary>
    [DataContract]
    public sealed class PermissionViewModel : IEquatable<PermissionViewModel>
    {
        /// <summary>
        /// Permission View Model Constructor
        /// </summary>
        public PermissionViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionViewModel" /> class.
        /// </summary>
        /// <param name="code">Code (required).</param>
        /// <param name="name">Name (required).</param>
        /// <param name="description">Description (required).</param>
        /// <param name="id">Id.</param>
        public PermissionViewModel(string code, string name, string description, int? id = null)
        {   
            Code = code;
            Name = name;
            Description = description;
            Id = id;
        }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class PermissionViewModel {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            return Equals((PermissionViewModel)obj);
        }

        /// <summary>
        /// Returns true if PermissionViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of PermissionViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PermissionViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Code == other.Code ||
                    Code != null &&
                    Code.Equals(other.Code)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) &&                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
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
                if (Code != null)
                {
                    hash = hash * 59 + Code.GetHashCode();
                }

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }

                if (Description != null)
                {
                    hash = hash * 59 + Description.GetHashCode();
                }

                if (Id != null)
                {
                    hash = hash * 59 + Id.GetHashCode();
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
        public static bool operator ==(PermissionViewModel left, PermissionViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PermissionViewModel left, PermissionViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
