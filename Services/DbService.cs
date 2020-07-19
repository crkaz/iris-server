using iris_server.Models;
using iris_server.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Services
{
    public static class DbService
    {
        public enum Collection { users, patients, carers, calendars, };


        public static async Task<bool> LookupPrimaryKey(DatabaseContext ctx, string key, Collection collection)
        {
            try
            {
                switch (collection)
                {
                    case Collection.users:
                        return await ctx.Users.FindAsync(key) != null;
                    case Collection.patients:
                        return await ctx.Patients.FindAsync(key) != null;
                    case Collection.carers:
                        return await ctx.Carers.FindAsync(key) != null;
                    case Collection.calendars:
                        return await ctx.Calendars.FindAsync(key) != null;
                    default:
                        throw new Exception("Unknown table.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return false;
        }


        public static async Task<IEntity> GetEntityByPrimaryKey(DatabaseContext ctx, string key, Collection collection)
        {
            try
            {
                switch (collection)
                {
                    case Collection.users:
                        return await ctx.Users.FindAsync(key);
                    case Collection.patients:
                        return await ctx.Patients.FindAsync(key);
                    case Collection.carers:
                        return await ctx.Carers.FindAsync(key);
                    case Collection.calendars:
                        return await ctx.Calendars.FindAsync(key);
                    default:
                        throw new Exception("Unknown table.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return null;
        }


        public static IEntity GetEntityByForiegnKey(DatabaseContext ctx, string key, Collection collection)
        {
            try
            {
                IEntity e;

                switch (collection)
                {
                    case Collection.patients:
                        e = ctx.Patients
                       .Where(p => p.User.ApiKey == key)
                       .FirstOrDefault();
                        break;
                    case Collection.carers:
                        e = ctx.Carers
                       .Where(c => c.User.ApiKey == key)
                       .FirstOrDefault();
                        break;
                    default:
                        throw new Exception("Unknown table.");
                }

                return e;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return null;
        }


        public static async Task<bool> DeleteEntityByPrimaryKey(DatabaseContext ctx, string key, Collection collection)
        {
            try
            {
                IEntity e = await GetEntityByPrimaryKey(ctx, key, collection);

                switch (collection)
                {
                    case Collection.patients:
                        if (key != "testpatient")
                            ctx.Users.Remove((e as Patient).User);
                        break;
                    case Collection.carers:
                        if (key != "testcarer")
                            ctx.Users.Remove((e as Carer).User);
                        break;
                    case Collection.calendars:
                        if (key != "testpatient")
                            ctx.Calendars.Remove(e as CalendarEntry);
                        break;
                    default:
                        throw new Exception("Unknown table.");
                }

                await ctx.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
            return false;
        }


        //public static async Task<ICollection<IEntity>> GetPaginatedCollection(DatabaseContext ctx, string pageIndex, string nItems)
        //{

        //}

        public static async Task<bool> UpdatePatientNotes(DatabaseContext ctx, string patientId, JObject notesJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(notesJson).ToObject<Dictionary<string, object>>();
                Patient patient = (Patient)await GetEntityByPrimaryKey(ctx, patientId, Collection.patients);
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
                Patient patient = (Patient)await GetEntityByPrimaryKey(ctx, patientId, Collection.patients);
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
                Carer carer = (Carer)GetEntityByForiegnKey(ctx, carerApiKey, Collection.carers);

                if (carer != null)
                {
                    Patient patient = (Patient)await GetEntityByPrimaryKey(ctx, patientId, Collection.patients);
                    if (patient != null)
                    {
                        PatientMessage messageObj = new PatientMessage() { Read = null, Sent = DateTime.Now, Title = title, Message = message };
                        patient.Messages.Add(messageObj);
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

                Carer carer = (Carer)GetEntityByForiegnKey(ctx, carerApiKey, Collection.carers);
                bool patientAssignedToThisCarer = carer.AssignedPatientIds.Contains(patientId);
                return patientAssignedToThisCarer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static async Task<bool> CreateCarer(DatabaseContext ctx, string carerEmail)
        {
            try
            {
                User user = new User() { Role = User.UserRole.formalcarer.ToString() };
                Carer carer = new Carer() { User = user, Email = carerEmail };
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


        public static async Task<bool> AddCalendarEntry(DatabaseContext ctx, string patientId, string entryId, Dictionary<string, object> jsonDict)
        {
            try
            {
                DateTime start = (DateTime)jsonDict["Start"];
                DateTime end = (DateTime)jsonDict["End"];
                int repetition = (int)(long)jsonDict["Repeat"];
                string description = (string)jsonDict["Description"];
                IList<string> reminders = (IList<string>)jsonDict["Reminders"];
                Patient patient = (Patient)await GetEntityByPrimaryKey(ctx, patientId, Collection.patients);
                CalendarEntry entry = new CalendarEntry() { Description = description, End = end, Start = start, Repeat = (CalendarEntry.Repetition)repetition, Reminders = reminders };
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


        public static async Task<ICollection<CalendarEntry>> GetCalendarEntries(DatabaseContext ctx, string patientId, string pageStr, string nItemsStr)
        {
            try
            {
                int page = Math.Abs(int.Parse(pageStr));
                int nItems = Math.Abs(int.Parse(nItemsStr));
                int startIndex = (page - 1) * nItems;
                var worker = await ctx.Patients.FindAsync(patientId);
                // Get only entries from todays date onwards.
                ICollection<CalendarEntry> entries = worker.CalendarEntries.Where(entry => entry.Start > DateTime.Now).ToList();
                nItems = nItems - ((page * nItems) - entries.Count);
                List<CalendarEntry> entriesList = entries.ToList();
                // Return paginated collection.
                return entriesList.GetRange(startIndex, nItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static ICollection<CalendarEntry> GetCalendarEntries(DatabaseContext ctx, string patientApiKey)
        {
            try
            {
                Patient patient = (Patient)GetEntityByForiegnKey(ctx, patientApiKey, Collection.patients);
                // Get only entries for today and tomorrow.
                ICollection<CalendarEntry> entries = patient.CalendarEntries.Where(entry => entry.Start.Day >= DateTime.Now.Day && entry.Start.Day <= DateTime.Now.Day + 1).ToList();
                return entries;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

    }
}
