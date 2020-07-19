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
                    bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, entryId);
                    if (patientAssignedToThisCarer)
                    {
                        bool success = DbService.AddCalendarEntry(_ctx, carerApiKey, entryId, jsonDict).GetAwaiter().GetResult();
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
        public IActionResult Put([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string entryId, [FromBody] JObject calendarJson)
        {
            try
            {
                CalendarEntry entry = DbService.GetCalendarEntryById(_ctx, entryId).GetAwaiter().GetResult();
                if (entry != null)
                {
                    bool carerAssignedToThisEntry = entry.Carer.Email == DbService.GetCarerByApiKey(_ctx, apiKey).Email;
                    if (carerAssignedToThisEntry)
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
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Get all future calender entries for a patient between todays date.
        // ..api/patient/calendar?id=..&date=
        [Authorize(Roles = "admin,formalcarer,informalcarer,patient")]
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            // apikey must match patient associated with the id OR a carer assigned to the patient with that id.
            return Ok("Endpoint works.");
        }


        // Get all calender entries from 'today' and 'tomorrow'.
        // ..api/patient/calendar?id=..&date=
        [HttpPost]
        [Authorize(Roles = "patient")]
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey)
        {
            // apikey must match patient associated with the id OR a carer assigned to the patient with that id.
            return Ok("Endpoint works.");
        }
    }
}