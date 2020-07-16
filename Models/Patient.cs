using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iris_server.Services;

namespace iris_server.Models
{
    public class Patient
    {
        public enum PatientStatus { offline, online, alert };

        // DB fields.
        [Key]
        public string Id { get; set; } // Primary Key.
        public virtual User User { get; set; } // Foreign Key.
        public string Status { get; set; }
        public string JsonPatientInfo { get; set; }
        public string JsonConfig { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }
        public virtual ICollection<StickyNote> Stickies { get; set; }
        public virtual ICollection<PatientMessage> Messages { get; set; }

        public Patient() { }
    }

    public static class PatientDatabaseAccess
    {
        public static Patient GetPatientById(DatabaseContext ctx, string id)
        {
            try
            {
                Patient patient = ctx.Patients.Find(id);
                return patient;
            }
            catch
            {
                // Log error.
                return null;
            }
        }

        public static Patient GetPatientByApiKey(DatabaseContext ctx, string apiKey)
        {
            try
            {
                foreach (Patient p in ctx.Patients)
                {
                    if (p.User.ApiKey == apiKey)
                    {
                        return p;
                    }
                }
            }
            catch
            {
                // Log error.
            }
            return null;
        }

        public static bool DeletePatientById(DatabaseContext ctx, string id)
        {
            try
            {
                Patient patient = ctx.Patients.Find(id);

                if (patient != null)
                {
                    //ArchiveDatabaseAcess.ArchiveUser(ctx, userToDelete);
                    // Don't delete the test record.
                    if (id != "testpatient")
                    {
                        ctx.Patients.Remove(patient);
                        ctx.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                // Log error.
            }
            return false;
        }
    }
}
