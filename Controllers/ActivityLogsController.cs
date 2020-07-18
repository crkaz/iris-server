using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class ActivityLogsController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ActivityLogsController(DatabaseContext context) : base(context) { }


        // Get the paginated logs of a patient.
        // ..api/activitylogs/get/?id=..&page=..&nitems=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Get([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            try
            {
                bool patientAssignedToCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);

                if (patientAssignedToCarer)
                {
                    ICollection<ActivityLog> logs = ActivityLogDatabaseAccess.GetLogs(_ctx, id, page, nItems);
                    string logsJson = JsonConvert.SerializeObject(logs);
                    return Ok(logsJson);
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


        // Add a new entry to the specified patient's activity logs.
        // ..api/activitylogs/post
        [Authorize(Roles = "patient")]
        public IActionResult Post([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject logEntryJson)
        {
            try
            {
                bool authorised = PatientDatabaseAccess.MatchApiKeyWithId(_ctx, apiKey, id);

                if (authorised)
                {
                    bool success = PatientDatabaseAccess.LogActivity(_ctx, id, logEntryJson);
                    if (success)
                    {
                        return Ok("Activity log added successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to log activity.");
                    }
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}