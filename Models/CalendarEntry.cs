using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class CalendarEntry
    {
        public enum Repetition { Never, Daily, Weekly, Monthly, Yearly };

        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Repetition Repeat { get; set; }
        public String Description { get; set; }
        public string JsonReminders { get; set; }

        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public CalendarEntry() { }
    }
}
