using System;
using System.Collections.Generic;
using System.Linq;
using iris_server.Models;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class MessageController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public MessageController(DbCtx context) : base(context) { }


        // Send a message to a patient.
        // ..api/message/post?id=
        [Authorize(Roles = _roles.carer)]
        public IActionResult Post([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject titleAndMessage)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    bool success = DbService.MessagePatient(_ctx, apiKey, id, titleAndMessage).GetAwaiter().GetResult();
                    if (success)
                    {
                        return Ok("Message sent successfully.");
                    }
                    return BadRequest("Failed to send message.");
                }
                return Unauthorized("You are not assigned to this patient.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get a patients unread messages.
        // ..api/message/get
        [Authorize(Roles = _roles.patient)]
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                Patient patient = (Patient)DbService.GetEntityByForeignKey(_ctx, apiKey, DbService.Collection.patients);
                // Get unread messages.
                IEnumerable<PatientMessage> unreadMessages = patient.Messages.Where(message => message.Read == null);
                return Ok(unreadMessages);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Update the 'read' date of a message. A GET request is used because no data is transmitted.
        // ..api/message/read?id=
        [HttpGet]
        [Authorize(Roles = _roles.patient)]
        public IActionResult Read([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string messageId)
        {
            try
            {
                PatientMessage message = _ctx.Messages.Find(messageId);
                if (message != null)
                {
                    message.Read = DateTime.Now;
                    _ctx.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}