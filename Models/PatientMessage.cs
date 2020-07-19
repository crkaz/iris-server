using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iris_server.Models
{
    public class PatientMessage
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        [ForeignKey("CarerId")]
        public string CarerId { get; set; }
        public DateTime Sent { get; set; }
        public DateTime? Read { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public PatientMessage() { }
    }
}
