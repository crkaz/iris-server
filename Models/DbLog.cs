using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class DbLog
    {
        // DB fields.
        [Key] // Make primary key.
        public string Id { get; set; } // Primary key.
        public string What { get; set; }
        public DateTime When { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
