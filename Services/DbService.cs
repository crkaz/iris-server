using iris_server.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Services
{
    public static class DbService
    {
        public static async Task<bool> LookupApiKey(DatabaseContext ctx, string apiKey)
        {
            try
            {
                User user = await ctx.Users.FindAsync(apiKey);
                bool userExists = user != null;
                return userExists;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return false;
        }


        public static async Task<User> GetUserByApiKey(DatabaseContext ctx, string apiKey)
        {
            try
            {
                User user = await ctx.Users.FindAsync(apiKey);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return null;
        }


        public static Patient GetPatientByApiKey(DatabaseContext ctx, string patientApiKey)
        {
            try
            {
                foreach (Patient p in ctx.Patients)
                {
                    if (p.User.ApiKey == patientApiKey)
                    {
                        return p;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static Carer GetCarerByApiKey(DatabaseContext ctx, string carerApiKey)
        {
            try
            {
                foreach (Carer c in ctx.Carers)
                {
                    if (c.User.ApiKey == carerApiKey)
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


        public static async Task<Patient> GetPatientById(DatabaseContext ctx, string patientId)
        {
            try
            {
                Patient patient = await ctx.Patients.FindAsync(patientId);
                return patient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static async Task<Carer> GetCarerById(DatabaseContext ctx, string carerId)
        {
            try
            {
                Carer carer = await ctx.Carers.FindAsync(carerId);
                return carer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static async Task<bool> MatchPatientApiKeyWithId(DatabaseContext ctx, string patientApiKey, string patientId)
        {
            try
            {
                Patient p1 = GetPatientByApiKey(ctx, patientApiKey);
                Patient p2 = await GetPatientById(ctx, patientId);

                if (p1 == p2)
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


        public static async Task<bool> MatchCarerApiKeyWithId(DatabaseContext ctx, string carerApiKey, string carerId)
        {
            try
            {
                Carer c1 = GetCarerByApiKey(ctx, carerApiKey);
                Carer c2 = await GetCarerById(ctx, carerId);

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


        public static async Task<bool> DeletePatientById(DatabaseContext ctx, string patientId)
        {
            try
            {
                Patient patient = await ctx.Patients.FindAsync(patientId);

                if (patient != null)
                {
                    //ArchiveDatabaseAcess.ArchiveUser(ctx, userToDelete);
                    // Don't delete the test record.
                    if (patientId != "testpatient")
                    {
                        ctx.Patients.Remove(patient);
                        await ctx.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static async Task<bool> DeleteCarer(DatabaseContext ctx, string carerId)
        {
            try
            {
                Carer carer = await ctx.Carers.FindAsync(carerId);
                if (carerId != "testcarer")
                {
                    ctx.Carers.Remove(carer);
                    await ctx.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static async Task<bool> UpdatePatientNotes(DatabaseContext ctx, string patientId, JObject notesJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(notesJson).ToObject<Dictionary<string, object>>();
                Patient patient = await GetPatientById(ctx, patientId);
                bool changes = false;

                foreach (string key in jsonDict.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "age":
                            int age = (int)(long)jsonDict["Age"];
                            patient.Notes.Age = (PatientNotes.AgeRange)age;
                            changes = true;
                            break;
                        case "diagnosis":
                            int diagnosis = (int)(long)jsonDict["Diagnosis"];
                            patient.Notes.Diagnosis = (PatientNotes.Severity)diagnosis;
                            changes = true;
                            break;
                        case "notes":
                            string notes = (string)jsonDict["Notes"];
                            patient.Notes.Notes = (string)notes;
                            changes = true;
                            break;
                    }
                }

                if (changes)
                {
                    await ctx.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        // TODO:
        public static async Task<bool> UpdatePatientConfig(DatabaseContext ctx, string patientId, JObject configJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(configJson).ToObject<Dictionary<string, object>>();
                Patient patient = await GetPatientById(ctx, patientId);
                bool changes = false;

                foreach (string key in jsonDict.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "inputs":
                            //int age = (int)(long)jsonDict["Age"];
                            //patient.Config.EnabledFeatures = (PatientNotes.AgeRange)age;
                            changes = true;
                            break;
                        case "falldetection":
                            changes = true;
                            break;
                        case "roomdetection":
                            changes = true;
                            break;
                        case "confusiondetection":
                            changes = true;
                            break;
                        case "stickynotes":
                            changes = true;
                            break;
                    }
                }

                if (changes)
                {
                    await ctx.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public static async Task<bool> MessagePatient(DatabaseContext ctx, string carerApiKey, string patientId, JObject messageJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(messageJson).ToObject<Dictionary<string, object>>();
                string title = (string)jsonDict["Title"];
                string message = (string)jsonDict["Message"];

                Carer sender = GetCarerByApiKey(ctx, carerApiKey);
                if (sender != null)
                {
                    Patient recipient = await GetPatientById(ctx, patientId);
                    if (recipient != null)
                    {
                        PatientMessage messageObj = new PatientMessage() { Carer = sender, Read = null, Sent = DateTime.Now, Title = title, Message = message };
                        recipient.Messages.Add(messageObj);
                        await ctx.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public static async Task<ICollection<ActivityLog>> GetLogs(DatabaseContext ctx, string patientId, string pageStr, string nItemsStr)
        {
            try
            {
                int page = Math.Abs(int.Parse(pageStr));
                int nItems = Math.Abs(int.Parse(nItemsStr));
                int startIndex = (page - 1) * nItems;
                var worker = await ctx.Patients.FindAsync(patientId);
                ICollection<ActivityLog> logs = worker.ActivityLogs;
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


        // TODO:
        public static async Task<bool> LogActivity(DatabaseContext ctx, HttpContext httpContext, Patient patient)
        {
            //try
            //{
            //    var jsonDict = JObject.FromObject(logEntryJson).ToObject<Dictionary<string, object>>();
            //    string caption = (string)jsonDict["Caption"];
            //    string description = (string)jsonDict["JsonDescription"];
            //    string location = (string)jsonDict["Location"];
            //    ActivityLog log = new ActivityLog();
            //    Patient patient = GetPatientById(ctx, id);

            //    patient.ActivityLogs.Add(log);
            //    await ctx.SaveChangesAsync();
            //    return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            return false;
        }

        public static bool PatientIsAssigned(DatabaseContext ctx, string carerApiKey, string patientId)
        {
            try
            {
                Carer carer = GetCarerByApiKey(ctx, carerApiKey);
                string[] assignedPatients = carer.AssignedPatientIds.ToArray<string>();
                if (assignedPatients.Contains(patientId))
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


        public static async Task<bool> CreateCarer(DatabaseContext ctx, string carerId)
        {
            try
            {
                User user = new User() { Role = User.UserRole.formalcarer.ToString() };
                Carer carer = new Carer() { User = user, Email = carerId };
                await ctx.Users.AddAsync(user);
                await ctx.Carers.AddAsync(carer);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        // TODO: 
        public static bool SendPasswordReset(DatabaseContext ctx, string carerId)
        {
            // Use firebase api.
            return false; // NOT IMPLEMENTED.
        }


        public static async Task<bool> AddCalendarEntry(DatabaseContext ctx, string carerApiKey, string patientId, Dictionary<string, object> jsonDict)
        {
            try
            {
                DateTime start = (DateTime)jsonDict["Start"];
                DateTime end = (DateTime)jsonDict["End"];
                int repetition = (int)(long)jsonDict["Repeat"];
                string description = (string)jsonDict["Description"];
                IList<string> reminders = (IList<string>)jsonDict["Reminders"];
                Carer carer = GetCarerByApiKey(ctx, carerApiKey);
                Patient patient = await GetPatientById(ctx, patientId);
                CalendarEntry entry = new CalendarEntry() { Carer = carer, Description = description, End = end, Start = start, Repeat = (CalendarEntry.Repetition)repetition, Reminders = reminders };
                patient.CalendarEntries.Add(entry);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static async Task<CalendarEntry> GetCalendarEntryById(DatabaseContext ctx, string entryId)
        {
            try
            {
                CalendarEntry entry = ctx.Calendars.Find(entryId);
                if (entry != null)
                {
                    return entry;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static async Task<bool> UpdateCalendarEntry(DatabaseContext ctx, string calendarId, Dictionary<string, object> jsonDict)
        {
            try
            {
                CalendarEntry entry = await ctx.Calendars.FindAsync(calendarId);
                bool changes = false;

                foreach (string key in jsonDict.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "start":
                            DateTime start = (DateTime)jsonDict["Start"];
                            entry.Start = start;
                            changes = true;
                            break;
                        case "end":
                            DateTime end = (DateTime)jsonDict["End"];
                            entry.End = end;
                            changes = true;
                            break;
                        case "repeat":
                            int repetition = (int)(long)jsonDict["Repeat"];
                            entry.Repeat = (CalendarEntry.Repetition)repetition;
                            changes = true;
                            break;
                        case "description":
                            string description = (string)jsonDict["Description"];
                            entry.Description = description;
                            changes = true;
                            break;
                        case "reminders":
                            IList<string> reminders = (IList<string>)jsonDict["Reminders"];
                            entry.Reminders = reminders;
                            changes = true;
                            break;
                    }
                }

                if (changes)
                {
                    await ctx.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
