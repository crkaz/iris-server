using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientStickiesController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientStickiesController(Models.UserContext context) : base(context) { }


        // Get the json serialised sticky notes for a patient.
        // ..api/patient/stickies?id=
        [HttpGet]
        [ActionName("stickies")]
        // [Authorize(Roles = "Admin,Carer,Patient")]
        public IActionResult StickiesGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Store/replace the json serialised sticky notes for a patient.
        // ..api/patient/stickies?id=
        [HttpGet]
        [ActionName("stickies")]
        // [Authorize(Roles = "Patient")]
        public IActionResult StickiesPost([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }
    }
}