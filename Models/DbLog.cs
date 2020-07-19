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
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        public string What { get; set; }
        public string Ip { get; set; }
        public DateTime When { get; set; }
        public int ResponseCode { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DbLog() { }

        public DbLog(string what, int responseCode, string ip)
        {
            this.What = what;
            this.When = DateTime.Now;
            this.ResponseCode = responseCode;
            this.Ip = ip;
        }
    }
}
