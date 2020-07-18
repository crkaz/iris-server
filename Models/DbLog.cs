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
        public bool Test { get; set; }
        public int ResponseCode { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DbLog() { }

        public DbLog(string what, int responseCode, bool testMode)
        {
            this.What = what;
            this.When = DateTime.Now;
            this.ResponseCode = responseCode;
            this.Test = testMode;
        }
    }
}
