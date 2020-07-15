using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class Log
    {
        // DB fields.
        [Key] // Make primary key.
        public string LogId { get; set; } // Primary key.
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
