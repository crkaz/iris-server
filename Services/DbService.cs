using iris_server.Models;
using iris_server.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Services
{
    public static class DbService
    {
        public enum Collection { users, patients, carers, calendars, activitylogs, stickies };


        public static async Task<IEntity> GetEntityByPrimaryKey(DbCtx ctx, string key, Collection collection)
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
                    case Collection.stickies:
                        return await ctx.Stickies.FindAsync(key);
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


        public static IEntity GetEntityByForeignKey(DbCtx ctx, string key, Collection collection)
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


        public static async Task<bool> DeleteEntityByPrimaryKey(DbCtx ctx, string key, Collection collection)
        {
            try
            {
                IEntity e = await GetEntityByPrimaryKey(ctx, key, collection);

                if (key != "testpatient" && key != "testcarer") // Avoid deleting test entries.
                {
                    switch (collection)
                    {
                        case Collection.patients:
                            ctx.Users.Remove((e as Patient).User);
                            break;
                        case Collection.carers:
                            ctx.Users.Remove((e as Carer).User);
                            break;
                        case Collection.calendars:
                            ctx.Calendars.Remove(e as CalendarEntry);
                            break;
                        case Collection.stickies:
                            ctx.Stickies.Remove(e as StickyNote);
                            break;
                        default:
                            throw new Exception("Unknown table.");
                    }
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

        // TODO: Implement visitor pattern.
        /// TODO: MOVE
        public static bool PatientIsAssigned(DbCtx ctx, string carerId, string patientId)
        {
            try
            {
                Carer carer = (Carer)GetEntityByForeignKey(ctx, carerId, Collection.carers);
                bool patientAssignedToThisCarer = carer.AssignedPatientIds.Contains(patientId);
                return patientAssignedToThisCarer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        /// TODO: MOVE
        public static async Task<bool> UpdatePatientNotes(DbCtx ctx, string patientId, JObject notesJson)
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

        /// TODO: IMPLEMENT
        public static async Task<bool> UpdatePatientConfig(DbCtx ctx, string patientId, JObject configJson)
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


        /// TODO: MOVE
        public static async Task<bool> MessagePatient(DbCtx ctx, string carerApiKey, string patientId, JObject messageJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(messageJson).ToObject<Dictionary<string, object>>();
                string title = (string)jsonDict["Title"];
                string message = (string)jsonDict["Message"];
                Carer carer = (Carer)GetEntityByForeignKey(ctx, carerApiKey, Collection.carers);

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


        public static async Task<bool> CreatePatientActivityLog(DbCtx ctx, string patientApiKey, JObject logJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(logJson).ToObject<Dictionary<string, object>>();
                string caption = (string)jsonDict["Caption"];
                string description = (string)jsonDict["JsonDescription"];
                string location = (string)jsonDict["Location"];
                ActivityLog log = new ActivityLog() { Caption = caption, JsonDescription = description, Location = location };
                Patient patient = (Patient)GetEntityByForeignKey(ctx, patientApiKey, Collection.patients);

                patient.ActivityLogs.Add(log);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        /// TODO: MOVE
        public static async Task<bool> CreateUser(DbCtx ctx, string primaryKey, User.UserRole role)
        {
            try
            {
                User user = new User() { Role = role.ToString() };
                await ctx.Users.AddAsync(user);

                if (role == User.UserRole.patient)
                {
                    Patient patient = new Patient() { User = user, Id = primaryKey };
                    await ctx.Patients.AddAsync(patient);
                }
                else
                {
                    Carer carer = new Carer() { User = user, Email = primaryKey };
                    await ctx.Carers.AddAsync(carer);

                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        /// TODO: MOVE
        public static async Task<bool> AddCalendarEntry(DbCtx ctx, string patientId, string entryId, Dictionary<string, object> jsonDict)
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


        /// TODO: MOVE
        public static async Task<bool> UpdateCalendarEntry(DbCtx ctx, string calendarId, Dictionary<string, object> jsonDict)
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


        /// TODO: MOVE
        public static async Task<ICollection<CalendarEntry>> GetCalendarEntries(DbCtx ctx, string patientId, string pageStr, string nItemsStr)
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


        /// TODO: MOVE
        public static async Task<ICollection<ActivityLog>> GetLogs(DbCtx ctx, string patientId, string pageStr, string nItemsStr)
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


        /// TODO: MOVE
        public static ICollection<CalendarEntry> GetCalendarEntries(DbCtx ctx, string patientApiKey)
        {
            try
            {
                Patient patient = (Patient)GetEntityByForeignKey(ctx, patientApiKey, Collection.patients);
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


        /// TODO: MOVE
        public static ICollection<StickyNote> GetStickyNotes(DbCtx ctx, string patientApiKey)
        {
            try
            {
                Patient patient = (Patient)GetEntityByForeignKey(ctx, patientApiKey, Collection.patients);
                ICollection<StickyNote> stickies = patient.Stickies;//.ToList();
                return stickies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        /// TODO: MOVE
        public static async Task<bool> AllocatePatient(DbCtx ctx, JObject patientAndCarerId)
        {
            try
            {
                string patientId = (string)patientAndCarerId["patient"];
                string carerEmail = (string)patientAndCarerId["carer"];
                bool assign = (bool)patientAndCarerId["assign"]; // Whether to assign or unassign.
                Patient patient = (Patient)await GetEntityByPrimaryKey(ctx, patientId, Collection.patients);
                Carer carer = (Carer)await GetEntityByPrimaryKey(ctx, carerEmail, Collection.carers);
                bool patientAlreadyAssigned = carer.AssignedPatientIds.Contains(patientId);
                if (assign)
                {
                    // Assign the patient to the carer if they are not assigned.
                    if (!patientAlreadyAssigned)
                    {
                        carer.AssignedPatientIds.Add(patientId);
                        await ctx.SaveChangesAsync();
                    }
                }
                else
                {
                    // Unassign the patient from the carer if they are assigned..
                    if (patientAlreadyAssigned)
                    {
                        if (patientId != "testpatient") // Don't remove test record.
                            carer.AssignedPatientIds.ToList().Remove(patientId);
                        await ctx.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        /// TODO: MOVE
        public static async Task<bool> ChangeCarerPermission(DbCtx ctx, JObject carerAndRole)
        {
            try
            {
                string carerEmail = (string)carerAndRole["email"];
                string role = (string)carerAndRole["role"];
                Carer carer = (Carer)await GetEntityByPrimaryKey(ctx, carerEmail, Collection.carers);

                carer.User.Role = role;
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        /// TODO: MOVE
        public static async Task<bool> AddStickyNote(DbCtx ctx, string patientApiKey, JObject stickyJson)
        {
            try
            {
                string stickyContent = (string)stickyJson["content"];
                float stickyScale = (float)stickyJson["scale"];
                if (!(string.IsNullOrWhiteSpace(stickyContent) || stickyScale == 0))
                {
                    Patient patient = (Patient)GetEntityByForeignKey(ctx, patientApiKey, Collection.patients);
                    StickyNote stickyNote = new StickyNote() { PatientId = patient.Id, Scale = stickyScale, Content = stickyContent };
                    patient.Stickies.Add(stickyNote);
                    await ctx.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }


        public static async Task<bool> UpdateStickyNote(DbCtx ctx, string stickyId, JObject stickyJson)
        {
            try
            {
                StickyNote sticky = (StickyNote)await GetEntityByPrimaryKey(ctx, stickyId, Collection.stickies);
                var jsonDict = JObject.FromObject(stickyJson).ToObject<Dictionary<string, object>>();
                bool changes = false;

                foreach (string key in jsonDict.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "content":
                            string content = (string)jsonDict[key];
                            sticky.Content = content;
                            changes = true;
                            break;

                        case "scale":
                            float scale = (float)jsonDict[key];
                            sticky.Scale = scale;
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
            }
            return false;
        }
    }
}
