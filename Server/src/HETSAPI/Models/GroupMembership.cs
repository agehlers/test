using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// Group Membership Database Model
    /// </summary>
    [MetaData (Description = "The users associated with a given group that has been defined in the application.")]
    public sealed class GroupMembership : AuditableEntity, IEquatable<GroupMembership>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public GroupMembership()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMembership" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a GroupMembership (required).</param>
        /// <param name="active">A flag indicating the User is active in the group. Set false to remove the user from the designated group. (required).</param>
        /// <param name="group">A foreign key reference to the system-generated unique identifier for a Group (required).</param>
        /// <param name="user">A foreign key reference to the system-generated unique identifier for a User (required).</param>
        public GroupMembership(int id, bool active, Group @group, User user)
        {   
            Id = id;
            Active = active;
            Group = @group;
            User = user;
        }

        /// <summary>
        /// A system-generated unique identifier for a GroupMembership
        /// </summary>
        /// <value>A system-generated unique identifier for a GroupMembership</value>
        [MetaData (Description = "A system-generated unique identifier for a GroupMembership")]
        public int Id { get; set; }
        
        /// <summary>
        /// A flag indicating the User is active in the group. Set false to remove the user from the designated group.
        /// </summary>
        /// <value>A flag indicating the User is active in the group. Set false to remove the user from the designated group.</value>
        [MetaData (Description = "A flag indicating the User is active in the group. Set false to remove the user from the designated group.")]
        public bool Active { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Group
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Group</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Group")]
        public Group Group { get; set; }
        
        /// <summary>
        /// Foreign key for Group 
        /// </summary>   
        [ForeignKey("Group")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Group")]
        public int? GroupId { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a User
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a User</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a User")]
        public User User { get; set; }
        
        /// <summary>
        /// Foreign key for User 
        /// </summary>   
        [ForeignKey("User")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for a User")]
        public int? UserId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class GroupMembership {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  Group: ").Append(Group).Append("\n");
            sb.Append("  User: ").Append(User).Append("\n");
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
            return obj.GetType() == GetType() && Equals((GroupMembership)obj);
        }

        /// <summary>
        /// Returns true if GroupMembership instances are equal
        /// </summary>
        /// <param name="other">Instance of GroupMembership to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GroupMembership other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Active == other.Active ||
                    Active.Equals(other.Active)
                ) &&                 
                (
                    Group == other.Group ||
                    Group != null &&
                    Group.Equals(other.Group)
                ) &&                 
                (
                    User == other.User ||
                    User != null &&
                    User.Equals(other.User)
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
                hash = hash * 59 + Active.GetHashCode();
                                   
                if (Group != null)
                {
                    hash = hash * 59 + Group.GetHashCode();
                }

                if (User != null)
                {
                    hash = hash * 59 + User.GetHashCode();
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
        public static bool operator ==(GroupMembership left, GroupMembership right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(GroupMembership left, GroupMembership right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
