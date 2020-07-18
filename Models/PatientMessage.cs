using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class PatientMessage
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        public virtual Carer Carer { get; set; } // Foreign key.
        public DateTime Sent { get; set; }
        public DateTime? Read { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public PatientMessage() { }
    }
}
