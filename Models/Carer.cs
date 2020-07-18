using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
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
        public static Carer GetCarerById(DatabaseContext ctx, string email)
        {
            try
            {
                Carer carer = ctx.Carers.Find(email);
                return carer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static bool CreateCarer(DatabaseContext ctx, string email)
        {
            try
            {
                User user = new User() { Role = User.UserRole.formalcarer.ToString() };
                Carer carer = new Carer() { User = user, Email = email };
                ctx.Users.Add(user);
                ctx.Carers.Add(carer);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static bool DeleteCarer(DatabaseContext ctx, string email)
        {
            try
            {
                Carer carer = ctx.Carers.Find(email);
                if (email != "testcarer")
                {
                    ctx.Carers.Remove(carer);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static bool SendPasswordReset(DatabaseContext ctx, string email)
        {
            // Use firebase api.
            return false; // NOT IMPLEMENTED.
        }


        public static bool MatchApiKeyWithId(DatabaseContext ctx, string apiKey, string id)
        {
            try
            {
                Carer c1 = GetCarerByApiKey(ctx, apiKey);
                Carer c2 = GetCarerById(ctx, id);

                if (c1 == c2)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

    }
}
