using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class StickiesController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public StickiesController(DatabaseContext context) : base(context) { }


        // Get the json serialised sticky notes for a patient.
        // ..api/stickies/get
        [Authorize(Roles = "patient")]
        public IActionResult Get([FromHeader(Name = "ApiKey")]string apiKey)
        {
            return Ok("Endpoint works.");
        }


        // Create a sticky notes for a patient.
        // ..api/stickies/post
        [Authorize(Roles = "patient")]
        public IActionResult Post([FromHeader(Name = "ApiKey")]string apiKey, [FromBody] JObject stickyJson)
        {
            return Ok("Endpoint works.");
        }


        // Modify a sticky note.
        // ..api/stickies/put/?id=
        [Authorize(Roles = "patient")]
        public IActionResult Put([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }


        // Delete a sticky note.
        // ..api/stickies/delete/?id=
        [Authorize(Roles = "patient")]
        public IActionResult Delete([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }

    }
}