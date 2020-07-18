using iris_server.Models;
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
        public static async Task<Patient> GetPatientById(DatabaseContext ctx, string id)
        {
            try
            {
                Patient patient = await ctx.Patients.FindAsync(id);
                return patient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        public static async Task<bool> MatchApiKeyWithId(DatabaseContext ctx, string apiKey, string id)
        {
            try
            {
                Patient p1 = GetPatientByApiKey(ctx, apiKey);
                Patient p2 = await GetPatientById(ctx, id);

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


        public static async Task<bool> DeletePatientById(DatabaseContext ctx, string id)
        {
            try
            {
                Patient patient = await ctx.Patients.FindAsync(id);

                if (patient != null)
                {
                    //ArchiveDatabaseAcess.ArchiveUser(ctx, userToDelete);
                    // Don't delete the test record.
                    if (id != "testpatient")
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


        public static async Task<bool> UpdatePatientNotes(DatabaseContext ctx, string id, JObject notesJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(notesJson).ToObject<Dictionary<string, object>>();
                Patient patient = await GetPatientById(ctx, id);
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
        public static async Task<bool> UpdatePatientConfig(DatabaseContext ctx, string id, JObject configJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(configJson).ToObject<Dictionary<string, object>>();
                Patient patient = await GetPatientById(ctx, id);
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

                Carer sender = CarerDatabaseAccess.GetCarerByApiKey(ctx, carerApiKey);
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
    }
}
