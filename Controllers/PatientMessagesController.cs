﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iris_server.Controllers
{
    public class PatientMessagesController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientMessagesController(Models.UserContext context) : base(context) { }


        // Add a message to the patients 'notes' field'.
        // ..api/patient/messages?id=
        [HttpPost]
        [ActionName("messages")]
        // [Authorize(Roles = "Admin,Carer")]
        public IActionResult MessagesPost([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] string titleAndMessage)
        {
            return Ok("Endpoint works.");
        }


        // Get a patients unread messages.
        // ..api/patient/messages?id=
        [HttpGet]
        [ActionName("messages")]
        // [Authorize(Roles = "Admin,Carer,Patient")]
        public IActionResult MessagesGet([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            return Ok("Endpoint works.");
        }
    }
}