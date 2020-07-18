using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public MessageController(DatabaseContext context) : base(context) { }


        // Send a message to a patient.
        // ..api/message/post?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public async Task<IActionResult> Post([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject titleAndMessage)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    bool success = await DbService.MessagePatient(_ctx, apiKey, id, titleAndMessage);
                    if (success)
                    {
                        return Ok("Message sent successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to send message.");
                    }
                }
                else
                {
                    return Unauthorized("You are not assigned to this patient.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get a patients unread messages.
        // ..api/message/get?id=
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool authorised = await DbService.MatchApiKeyWithId(_ctx, apiKey, id);

                if (authorised)
                {
                    Patient patient = DbService.GetPatientByApiKey(_ctx, apiKey);

                    // Get unread messages.
                    IEnumerable<PatientMessage> unreadMessages = patient.Messages.Where(message => message.Read == null);
                    return Ok(unreadMessages);
                }
                else
                {
                    return Unauthorized("Credentials do not match.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}