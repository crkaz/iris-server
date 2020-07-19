using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iris_server.Models
{
    public class PatientNotes
    {
        public enum Severity { mild, moderate, severe, ad };
        public enum AgeRange { _25under, _26_35, _36_45, _46_55, _56_65, _66_70, _71_75, _76_80, _81_85_, _86_90, _90plus };

        [Key]
        public string Id { get; set; } // Primary key.
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        public AgeRange Age { get; set; }
        public Severity Diagnosis { get; set; }
        public string Notes { get; set; }

        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public PatientNotes() { }

        public PatientNotes(AgeRange ageRange, Severity severity, string notes)
        {
            this.Age = ageRange;
            this.Diagnosis = severity;
            this.Notes = notes;
        }
    }
}
