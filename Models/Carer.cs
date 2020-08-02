using iris_server.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iris_server.Models
{
    public class Carer : IEntity
    {
        // DB fields.
        [Key]
        public string Email { get; set; } // Primary Key
        [Required]
        public virtual User User { get; set; } // Foreign Key.
        public string AssignedPatientIds { get; set; } // csList
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }

        public Carer() { }
    }
}
