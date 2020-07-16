using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientNotesController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientNotesController(Models.DatabaseContext context) : base(context) { }


        // Edit a patient's 'notes' field'.
        // ..api/patient/notes?id=
        [HttpPut]
        [ActionName("notes")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult NotesPut([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string notes)
        {
            return Ok("Endpoint works.");
        }


        // Get a patient's 'notes' field'.
        // ..api/patient/notes?id=
        [HttpGet]
        [ActionName("notes")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult NotesGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }
    }
}