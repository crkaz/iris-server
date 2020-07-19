using iris_server.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iris_server.Models
{
    public class ActivityLog : IEntity
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        public DateTime DateTime { get; set; }
        public string Caption { get; set; }
        public string Location { get; set; }
        public string JsonDescription { get; set; }

        public ActivityLog() { }
    }
}
