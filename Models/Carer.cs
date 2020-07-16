using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class Carer
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Patient> AssignedPatients { get; set; }
        public virtual ICollection<CalendarEntry> CreatedAppointments { get; set; }
        public virtual ICollection<PatientMessage> SentMessages { get; set; }
        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public Carer() { }
    }
}
