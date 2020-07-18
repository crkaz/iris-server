using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Models
{
    public class Carer
    {
        // DB fields.
        [Key]
        public string Email { get; set; } // Primary Key
        public virtual User User { get; set; } // Foreign Key.
        public IList<string> AssignedPatientIds { get; set; }

        public Carer() { }
    }

    public static class CarerDatabaseAccess
    {
        public static Carer GetCarerByApiKey(DatabaseContext ctx, string apiKey)
        {
            try
            {
                foreach (Carer c in ctx.Carers)
                {
                    if (c.User.ApiKey == apiKey)
                    {
                        return c;
                    }
                }
            }
            catch
            {
                // Log error.
            }
            return null;
        }

        public static bool PatientIsAssigned(DatabaseContext ctx, string apiKey, string id)
        {
            try
            {
                Carer carer = GetCarerByApiKey(ctx, apiKey);
                string[] assignedPatients = carer.AssignedPatientIds.ToArray<string>();
                if (assignedPatients.Contains(id))
                {
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
