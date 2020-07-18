using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iris_server.Models
{
    public class ActivityLog
    {
        // DB fields.
        [Key] // Make primary key via EF convention.
        public string Id { get; set; } // Primary key.
        public DateTime DateTime { get; set; }
        public string Caption { get; set; }
        public string Location { get; set; }
        public string JsonDescription { get; set; }

        public ActivityLog() { }

        public ActivityLog(string caption, string description, string location)
        {
            this.Caption = caption;
            this.JsonDescription = description;
            this.Location = location;
            this.DateTime = DateTime.Now;
        }
    }

    public static class ActivityLogDatabaseAccess
    {
        public static ICollection<ActivityLog> GetLogs(DatabaseContext ctx, string patientId, string pageStr, string nItemsStr)
        {
            try
            {
                int page = Math.Abs(int.Parse(pageStr));
                int nItems = Math.Abs(int.Parse(nItemsStr));
                int startIndex = (page - 1) * nItems;
                ICollection<ActivityLog> logs = ctx.Patients.Find(patientId).ActivityLogs;
                nItems = nItems - ((page * nItems) - logs.Count);
                List<ActivityLog> logsList = logs.ToList();
                // Return paginated collection.
                return logsList.GetRange(startIndex, nItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
