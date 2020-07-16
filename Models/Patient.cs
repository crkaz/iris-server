using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class Patient
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; }
        public virtual User User { get; set; }
        public string JsonPatientInfo { get; set; }
        public string JsonConfig { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }
        public virtual ICollection<StickyNote> Stickies { get; set; }
        public virtual ICollection<PatientMessage> Messages { get; set; }
        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public Patient() { }
    }
}
