using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iris_server.Models
{
    public class StickyNote
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        public string Content { get; set; }
        public int Scale { get; set; }

        public StickyNote() { }
    }
}
