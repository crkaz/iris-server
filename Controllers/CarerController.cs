using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

namespace iris_server.Controllers
{
    public class CarerController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public CarerController(DatabaseContext context) : base(context) { }


        // Create a new user in the system.
        // ..api/carer/add
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] string email)
        {
            return Ok("Endpoint works.");
        }

        // Return the collection of users registered in the system.
        // ..api/carer/list
        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public IActionResult List([FromHeader(Name = "ApiKey")] string apiKey)
        {
            return Ok("Endpoint works.");
        }


        // Send a password-reset email to the specified user.
        // ..api/carer/reset?id=
        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public IActionResult Reset([FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Delete a user from the system, given their id.
        // ..api/carer/delete?id=
        [HttpDelete]
        // [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Assign a patient to a carer as specified in the body.
        // ..api/carer/assign
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public IActionResult Assign([FromBody] string patientAndCarerId)
        {
            return Ok("Endpoint works.");
        }


        // Change the role/permissions of the user specified in the body.
        // ..api/carer/role
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public IActionResult Role([FromBody] string carerAndRole)
        {
            return Ok("Endpoint works.");
        }
    }
}
