using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientController(Models.UserContext context) : base(context) { }


        // Delete a user from the system, given their id.
        // ..api/patient/delete?id=
        [HttpDelete]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Return a collection patients specified by their ids in an array in the URI.
        // ..api/patient/list?id=..&id=
        [HttpGet]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult List([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Update the status of the patient (e.g. online, offline, alert).
        // ..api/patient/status?id=
        [HttpPut]
        // [Authorize(Roles = "Patient")]
        public IActionResult Status([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string status)
        {
            return Ok("Endpoint works.");
        }
    }
}