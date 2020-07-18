using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

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
        public virtual PatientNotes Notes { get; set; }
        public virtual PatientConfig Config { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<CalendarEntry> CalendarEntries { get; set; }
        public virtual ICollection<StickyNote> Stickies { get; set; }
        public virtual ICollection<PatientMessage> Messages { get; set; }

        public Patient() { }
    }

    //public static class DbService
    //{
        //    public static Patient GetPatientById(DatabaseContext ctx, string id)
        //    {
        //        try
        //        {
        //            Patient patient = ctx.Patients.Find(id);
        //            return patient;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //        return null;
        //    }

        //    public static Patient GetPatientByApiKey(DatabaseContext ctx, string apiKey)
        //    {
        //        try
        //        {
        //            foreach (Patient p in ctx.Patients)
        //            {
        //                if (p.User.ApiKey == apiKey)
        //                {
        //                    return p;
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //        return null;
        //    }

        //    public static bool MatchApiKeyWithId(DatabaseContext ctx, string apiKey, string id)
        //    {
        //        try
        //        {
        //            Patient p1 = GetPatientByApiKey(ctx, apiKey);
        //            Patient p2 = GetPatientById(ctx, id);

        //            if (p1 == p2)
        //            {
        //                return true;
        //            }

        //            return false;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //        return false;
        //    }

        //    public static bool DeletePatientById(DatabaseContext ctx, string id)
        //    {
        //        try
        //        {
        //            Patient patient = ctx.Patients.Find(id);

        //            if (patient != null)
        //            {
        //                //ArchiveDatabaseAcess.ArchiveUser(ctx, userToDelete);
        //                // Don't delete the test record.
        //                if (id != "testpatient")
        //                {
        //                    ctx.Patients.Remove(patient);
        //                    ctx.SaveChanges();
        //                }
        //                return true;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //        return false;
        //    }

        //    public static bool UpdatePatientNotes(DatabaseContext ctx, string id, JObject notesJson)
        //    {
        //        try
        //        {
        //            var jsonDict = JObject.FromObject(notesJson).ToObject<Dictionary<string, object>>();
        //            Patient patient = GetPatientById(ctx, id);
        //            bool changes = false;

        //            foreach (string key in jsonDict.Keys)
        //            {
        //                switch (key.ToLower())
        //                {
        //                    case "age":
        //                        int age = (int)(long)jsonDict["Age"];
        //                        patient.Notes.Age = (PatientNotes.AgeRange)age;
        //                        changes = true;
        //                        break;
        //                    case "diagnosis":
        //                        int diagnosis = (int)(long)jsonDict["Diagnosis"];
        //                        patient.Notes.Diagnosis = (PatientNotes.Severity)diagnosis;
        //                        changes = true;
        //                        break;
        //                    case "notes":
        //                        string notes = (string)jsonDict["Notes"];
        //                        patient.Notes.Notes = (string)notes;
        //                        changes = true;
        //                        break;
        //                }
        //            }

        //            if (changes)
        //            {
        //                ctx.SaveChanges();
        //                return true;
        //            }

        //            return false;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            return false;
        //        }
        //    }

        //    // TODO:
        //    public static bool UpdatePatientConfig(DatabaseContext ctx, string id, JObject configJson)
        //    {
        //        try
        //        {
        //            var jsonDict = JObject.FromObject(configJson).ToObject<Dictionary<string, object>>();
        //            Patient patient = GetPatientById(ctx, id);
        //            bool changes = false;

        //            foreach (string key in jsonDict.Keys)
        //            {
        //                switch (key.ToLower())
        //                {
        //                    case "inputs":
        //                        //int age = (int)(long)jsonDict["Age"];
        //                        //patient.Config.EnabledFeatures = (PatientNotes.AgeRange)age;
        //                        changes = true;
        //                        break;
        //                    case "falldetection":
        //                        changes = true;
        //                        break;
        //                    case "roomdetection":
        //                        changes = true;
        //                        break;
        //                    case "confusiondetection":
        //                        changes = true;
        //                        break;
        //                    case "stickynotes":
        //                        changes = true;
        //                        break;
        //                }
        //            }

        //            if (changes)
        //            {
        //                ctx.SaveChanges();
        //                return true;
        //            }
        //            return false;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            return false;
        //        }
        //    }


        //    public static bool MessagePatient(DatabaseContext ctx, string carerApiKey, string patientId, JObject messageJson)
        //    {
        //        try
        //        {
        //            var jsonDict = JObject.FromObject(messageJson).ToObject<Dictionary<string, object>>();
        //            string title = (string)jsonDict["Title"];
        //            string message = (string)jsonDict["Message"];

        //            Carer sender = CarerDatabaseAccess.GetCarerByApiKey(ctx, carerApiKey);
        //            if (sender != null)
        //            {
        //                Patient recipient = GetPatientById(ctx, patientId);
        //                if (recipient != null)
        //                {
        //                    PatientMessage messageObj = new PatientMessage() { Carer = sender, Read = null, Sent = DateTime.Now, Title = title, Message = message };
        //                    recipient.Messages.Add(messageObj);
        //                    ctx.SaveChanges();
        //                    return true;
        //                }
        //            }

        //            return false;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            return false;
        //        }
        //    }


        //    public static ICollection<ActivityLog> GetLogs(DatabaseContext ctx, string patientId, string pageStr, string nItemsStr)
        //    {
        //        try
        //        {
        //            int page = Math.Abs(int.Parse(pageStr));
        //            int nItems = Math.Abs(int.Parse(nItemsStr));
        //            int startIndex = (page - 1) * nItems;
        //            ICollection<ActivityLog> logs = ctx.Patients.Find(patientId).ActivityLogs;
        //            nItems = nItems - ((page * nItems) - logs.Count);
        //            List<ActivityLog> logsList = logs.ToList();
        //            // Return paginated collection.
        //            return logsList.GetRange(startIndex, nItems);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }

        //        return null;
        //    }


        //    public static bool LogActivity(DatabaseContext ctx, HttpContext httpContext, Patient patient)
        //    {
        //        try
        //        {
        //            var jsonDict = JObject.FromObject(logEntryJson).ToObject<Dictionary<string, object>>();
        //            string caption = (string)jsonDict["Caption"];
        //            string description = (string)jsonDict["JsonDescription"];
        //            string location = (string)jsonDict["Location"];
        //            ActivityLog log = new ActivityLog();
        //            Patient patient = GetPatientById(ctx, id);

        //            patient.ActivityLogs.Add(log);
        //            ctx.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //        return false;
        //    }
    //}
}
