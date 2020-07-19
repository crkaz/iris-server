using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;
using iris_server.Services;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace iris_server.Controllers
{
    public class CalendarController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public CalendarController(DatabaseContext context) : base(context) { }


        // Add a new calender entry to the patient's calendar.
        // ..api/patient/calendar?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Post([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string entryId, [FromBody] JObject calendarJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(calendarJson).ToObject<Dictionary<string, object>>();
                DateTime start = (DateTime)jsonDict["Start"];
                DateTime end = (DateTime)jsonDict["End"];

                bool validEntry = (start != null && end != null) && (start > DateTime.Now.AddMinutes(5)) && (start <= end);
                if (validEntry)
                {
                    CalendarEntry entry = (CalendarEntry)DbService.GetEntityByPrimaryKey(_ctx, entryId, DbService.Collection.calendars).GetAwaiter().GetResult();
                    bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, entry.PatientId);
                    if (patientAssignedToThisCarer)
                    {
                        bool success = DbService.AddCalendarEntry(_ctx, entry.PatientId, entryId, jsonDict).GetAwaiter().GetResult();
                        if (success)
                        {
                            return Ok("Successfully added calendar entry.");
                        }
                        else
                        {
                            return BadRequest("Failed to add calendar entry.");
                        }
                    }
                    else
                    {
                        return Unauthorized("You are not assigned to this patient.");
                    }
                }
                else
                {
                    return BadRequest("Invalid start date.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Edit a calender entry given its id.
        // ..api/patient/calendar?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Put([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string entryId, [FromBody] JObject calendarJson)
        {
            try
            {
                CalendarEntry entry = (CalendarEntry)DbService.GetEntityByPrimaryKey(_ctx, entryId, DbService.Collection.calendars).GetAwaiter().GetResult();
                if (entry != null)
                {
                    bool carerAssignedToPatient = DbService.PatientIsAssigned(_ctx, carerApiKey, entry.PatientId);
                    if (carerAssignedToPatient)
                    {
                        var jsonDict = JObject.FromObject(calendarJson).ToObject<Dictionary<string, object>>();
                        DateTime start = (DateTime)jsonDict["Start"];
                        DateTime end = (DateTime)jsonDict["End"];

                        bool validEntry = (start != null && end != null) && (start > DateTime.Now.AddMinutes(5)) && (start <= end);
                        if (validEntry)
                        {
                            bool success = DbService.UpdateCalendarEntry(_ctx, entryId, jsonDict).GetAwaiter().GetResult();
                            if (success)
                            {
                                return Ok("Calendar entry updated successfully.");
                            }
                            else
                            {
                                return BadRequest("Failed to add calendar entry.");
                            }
                        }
                        else
                        {
                            return BadRequest("Invalid start date.");
                        }

                    }
                    else
                    {
                        return Unauthorized("You are not assigned to this patient.");
                    }
                }
                else
                {
                    return NotFound("Could not find an entry with that id.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string entryId)
        {
            try
            {
                bool entryExists = DbService.GetCalendarEntryById(_ctx, entryId).GetAwaiter().GetResult() != null;
                if (entryExists)
                {
                    bool success = DbService.DeleteEntityByPrimaryKey(_ctx, entryId, DbService.Collection.calendars).GetAwaiter().GetResult();
                    if (success)
                    {
                        return Ok("Calendar entry deleted successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to delete carer.");
                    }
                }
                else
                {
                    return NotFound("Could not find an entry with that id.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get all future calender entries for a patient between todays date.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [Authorize(Roles = "admin,formalcarer,informalcarer,patient")]
        public IActionResult CarerGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            try
            {
                bool patientAssignedToCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToCarer)
                {
                    ICollection<CalendarEntry> entries = DbService.GetCalendarEntries(_ctx, id, page, nItems).GetAwaiter().GetResult();
                    string entriesJson = JsonConvert.SerializeObject(entries);
                    return Ok(entriesJson);
                }
                else
                {
                    return Unauthorized("You are not assigned to this patient.");
                }
                // Return logs as a paginated json collection
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get all calender entries from 'today' and 'tomorrow'.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [Authorize(Roles = "patient")]
        public IActionResult PatientGet([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                ICollection<CalendarEntry> entries = DbService.GetCalendarEntries(_ctx, apiKey);
                string entriesJson = JsonConvert.SerializeObject(entries);
                return Ok(entriesJson);
                // Return logs as a paginated json collection
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}