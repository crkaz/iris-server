using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class CalendarController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public CalendarController(DatabaseContext context) : base(context) { }


        // Add a new calender entry to the patient's calendar.
        // ..api/patient/calendar?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Post([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject calendarJson)
        {
            return Ok("Endpoint works.");
        }


        // Edit a calender entry given its id.
        // ..api/patient/calendar?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Put([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject calendarJson)
        {
            return Ok("Endpoint works.");
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Get all calender entries from a given date until the future.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [Authorize(Roles = "admin,formalcarer,informalcarer,patient")]
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "date")] string date)
        {
            // apikey must match patient associated with the id OR a carer assigned to the patient with that id.
            return Ok("Endpoint works.");
        }
    }
}