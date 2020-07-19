using iris_server.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iris_server.Models
{
    public class User : IEntity
    {
        public enum UserRole { admin, formalcarer, informalcarer, patient };

        // DB fields.
        [Key] // Make primary key via EF convention.
        public string ApiKey { get; set; } // Primary key.
        public string Role { get; set; }
        public virtual ICollection<DbLog> DbLogs { get; set; }
        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public User() { }
    }
}
