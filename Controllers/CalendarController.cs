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
        public CalendarController(DbCtx context) : base(context) { }


        // Add a new calender entry to the patient's calendar.
        // ..api/patient/calendar?id=
        [Authorize(Roles = _roles.carer)]
        public IActionResult Post([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string patientId, [FromBody] JObject calendarJson)
        {
            try
            {
                var jsonDict = JObject.FromObject(calendarJson).ToObject<Dictionary<string, object>>();
                DateTime start = DateTime.Parse((string)jsonDict["Start"]);
                DateTime end;
                bool endProvided = DateTime.TryParse((string)jsonDict["End"], out end);
                if (!endProvided)
                    end = start;
                
                bool validEntry = (start != null && end != null) && (start > DateTime.Now.AddMinutes(5)) && (start <= end);
                if (validEntry)
                {
                    bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, patientId);
                    if (patientAssignedToThisCarer)
                    {
                        bool success = DbService.AddCalendarEntry(_ctx, patientId, jsonDict).GetAwaiter().GetResult();
                        if (success)
                        {
                            return Ok("Successfully added calendar entry.");
                        }
                        return BadRequest("Failed to add calendar entry.");
                    }
                    return Unauthorized("You are not assigned to this patient.");
                }
                return BadRequest("Invalid start date.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Edit a calender entry given its id.
        // ..api/patient/calendar?id=
        [Authorize(Roles = _roles.carer)]
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
                            return BadRequest("Failed to update calendar entry.");
                        }
                        return BadRequest("Invalid start date.");
                    }
                    return Unauthorized("You are not assigned to this patient.");
                }
                return NotFound("Could not find an entry with that id.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=
        [Authorize(Roles = _roles.carer)]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string entryId)
        {
            try
            {
                CalendarEntry entry = (CalendarEntry)DbService.GetEntityByPrimaryKey(_ctx, entryId, DbService.Collection.calendars).GetAwaiter().GetResult();
                bool entryExists = DbService.GetEntityByPrimaryKey(_ctx, entryId, DbService.Collection.calendars).GetAwaiter().GetResult() != null;
                if (entryExists)
                {
                    Carer carer = (Carer)DbService.GetEntityByForeignKey(_ctx, carerApiKey, DbService.Collection.carers);
                    bool authorised = (carer.AssignedPatientIds != null && carer.AssignedPatientIds.Contains(entry.PatientId));
                    if (authorised)
                    {
                        bool success = DbService.DeleteEntityByPrimaryKey(_ctx, entryId, DbService.Collection.calendars).GetAwaiter().GetResult();
                        if (success)
                        {
                            return Ok("Calendar entry deleted successfully.");
                        }
                        return BadRequest("Failed to delete calendar entry.");
                    }
                    return Unauthorized("You are not assigned to that patient.");
                }
                return NotFound("Could not find an entry with that id.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get all future calender entries for a patient between todays date.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [Authorize(Roles = _roles.carer)]
        public IActionResult CarerGet([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string patientId, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            try
            {
                bool patientAssignedToCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, patientId);
                if (patientAssignedToCarer)
                {
                    ICollection<CalendarEntry> entries = DbService.GetCalendarEntries(_ctx, patientId, page, nItems).GetAwaiter().GetResult();
                    string entriesJson = JsonConvert.SerializeObject(entries);
                    return Ok(entriesJson);
                    // Return logs as a paginated json collection
                }
                return Unauthorized("You are not assigned to this patient.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get all calender entries from 'today' and 'tomorrow'.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [Authorize(Roles = _roles.patient)]
        public IActionResult PatientGet([FromHeader(Name = "ApiKey")] string patientApiKey)
        {
            try
            {
                ICollection<CalendarEntry> entries = DbService.GetCalendarEntries(_ctx, patientApiKey);
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