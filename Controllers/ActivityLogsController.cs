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


        



        // TODO: MAKE MIDDLEWARE == 1 less request
        //// Add a new entry to the specified patient's activity logs.
        //// ..api/activitylogs/post
        //[Authorize(Roles = "patient")]
        //public IActionResult Post([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject logEntryJson)
        //{
        //    try
        //    {
        //        bool authorised = DbService.MatchApiKeyWithId(_ctx, apiKey, id);

        //        if (authorised)
        //        {
        //            bool success = DbService.LogActivity(_ctx, id, logEntryJson);
        //            if (success)
        //            {
        //                return Ok("Activity log added successfully.");
        //            }
        //            else
        //            {
        //                return BadRequest("Failed to log activity.");
        //            }
        //        }
        //        else
        //        {
        //            return Unauthorized("Invalid credentials.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
    }
}