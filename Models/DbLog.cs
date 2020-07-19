using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public DbLog() { }

        public DbLog(string what)
        {
            this.What = what;
            this.When = DateTime.Now;
        }
    }
}
