using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientLogsController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientLogsController(Models.DatabaseContext context) : base(context) { }


        // Get the paginated logs of a patient.
        // ..api/patient/logs?id=..&page=..&nitems=
        [HttpGet]
        [ActionName("logs")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult LogsGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            return Ok("Endpoint works.");
        }


        // Add a new entry to the specified patient's activity logs.
        // ..api/patient/logs?id=
        [HttpPost]
        [ActionName("logs")]
        // [Authorize(Roles = "Patient")]
        public IActionResult LogsPost([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string logEntry)
        {
            return Ok("Endpoint works.");
        }
    }
}