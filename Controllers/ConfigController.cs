using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

namespace iris_server.Controllers
{
    public class ConfigController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ConfigController(DatabaseContext context) : base(context) { }


        // Modify a parameter of the patient's device configuration.
        // ..api/patient/config?id=
        [HttpPut]
        [ActionName("config")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult ConfigPut([FromQuery(Name = "id")] string id, [FromBody] string optionAndValue)
        {
            return Ok("Endpoint works.");
        }


        // Get the patient's device configuration.
        // ..api/patient/config?id=
        [HttpGet]
        [ActionName("config")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult ConfigGet([FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }
    }
}