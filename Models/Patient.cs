using iris_server.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iris_server.Models
{
    public class Patient : IEntity
    {
        public enum PatientStatus { offline, online, alert };

        // DB fields.
        [Key]
        public string Id { get; set; } // Primary Key.
        [Required]
        public virtual User User { get; set; } // Foreign Key.
        public string Status { get; set; }
        public virtual PatientNotes Notes { get; set; }
        public virtual PatientConfig Config { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }
        public virtual ICollection<StickyNote> Stickies { get; set; }
        public virtual ICollection<PatientMessage> Messages { get; set; }

        public Patient() { }
    }
}
