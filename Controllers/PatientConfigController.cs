using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientConfigController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientConfigController(Models.UserContext context) : base(context) { }


        // Modify a parameter of the patient's device configuration.
        // ..api/patient/config?id=
        [HttpPut]
        [ActionName("config")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult ConfigPut([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string optionAndValue)
        {
            return Ok("Endpoint works.");
        }


        // Get the patient's device configuration.
        // ..api/patient/config?id=
        [HttpGet]
        [ActionName("config")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult ConfigGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }
    }
}