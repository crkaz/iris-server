using iris_server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iris_server.Models
{
    public class StickyNote : IEntity
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        public string Content { get; set; }
        public float Scale { get; set; }

        public StickyNote() { }
    }
}
