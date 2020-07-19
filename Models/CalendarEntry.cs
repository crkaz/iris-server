using iris_server.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iris_server.Models
{
    public class CalendarEntry : IEntity
    {
        public enum Repetition { Never, Daily, Weekly, Monthly, Yearly };

        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        [ForeignKey("CarerEmail")]
        public string CarerEmail { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Repetition Repeat { get; set; }
        public string Description { get; set; }
        public IList<string> Reminders { get; set; }

        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public CalendarEntry() { }
    }
}
