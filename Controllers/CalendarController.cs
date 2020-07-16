using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

namespace iris_server.Controllers
{
    public class CalendarController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public CalendarController(DatabaseContext context) : base(context) { }


        // Add a new calender entry to the patient's calendar.
        // ..api/patient/calendar?id=
        [HttpPost]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult CalendarPost([FromQuery(Name = "id")] string id, [FromBody] string calendarEntry)
        {
            return Ok("Endpoint works.");
        }


        // Edit a calender entry given its id.
        // ..api/patient/calendar?id=
        [HttpPut]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult CalendarPut([FromQuery(Name = "id")] string id, [FromBody] string calendarEntry)
        {
            return Ok("Endpoint works.");
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=
        [HttpDelete]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult CalendarDelete([FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Delete a calender entry given its id.
        // ..api/patient/calendar?id=..&date=
        [HttpGet]
        [ActionName("calendar")]
        // [Authorize(Roles = "Admin,Carer,Patient")]
        public IActionResult CalendarGet([FromQuery(Name = "id")] string id, [FromQuery(Name = "date")] string date)
        {
            return Ok("Endpoint works.");
        }
    }
}