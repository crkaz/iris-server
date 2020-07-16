using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class ActivityLog
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        public DateTime DateTime { get; set; }
        public string Caption{ get; set; }
        public string JsonDescription { get; set; }

        public ActivityLog() { }
    }
}
