using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientCalendarController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientCalendarController(Models.UserContext context) : base(context) { }


        // Add a new calender entry to the patient's calendar.
        // ..api/patient/calendar?id=
        [HttpPost]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult CalendarPost([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string calendarEntry)
        {
            return Ok("Endpoint works.");
        }


        // Edit a calender entry given its id.
        // ..api/patient/calendar?id=
        [HttpPut]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult CalendarPut([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string calendarEntry)
        {
            return Ok("Endpoint works.");
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=
        [HttpDelete]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult CalendarDelete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer,Patient")]
        public IActionResult CalendarGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "date")] string date)
        {
            return Ok("Endpoint works.");
        }
    }
}