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
        public IList<string> AssignedPatientIds { get; set; }
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }

        public Carer() { }

        // TODO: 
        public bool SendPasswordReset()
        {
            // Use firebase api.
            return false; // NOT IMPLEMENTED.
        }
    }
}
