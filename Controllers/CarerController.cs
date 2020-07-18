using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Rewrite.Internal;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Remotion.Linq.Utilities;
using System.Threading.Tasks;
using iris_server.Services;

namespace iris_server.Controllers
{
    public class CarerController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public CarerController(DatabaseContext context) : base(context) { }


        // Create a new user in the system.
        // ..api/carer/post
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] string email)
        {
            try
            {
                bool emailValid = (!string.IsNullOrWhiteSpace(email) && email.Contains("@"));
                if (!emailValid)
                {
                    return BadRequest("Invalid email format.");
                }
                else
                {
                    bool emailAlreadyExists = DbService.GetCarerById(_ctx, email).GetAwaiter().GetResult() != null;
                    if (emailAlreadyExists)
                    {
                        return BadRequest("Email address already in use.");
                    }
                    else
                    {
                        bool success = DbService.CreateCarer(_ctx, email).GetAwaiter().GetResult();
                        if (success)
                        {
                            return Ok("New carer added successfully.");
                        }
                        else
                        {
                            return BadRequest("Failed to create new carer.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // Return the collection of users registered in the system.
        // ..api/carer/get
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Get()
        {
            try
            {
                string carers = JsonConvert.SerializeObject(_ctx.Carers);
                return Ok(carers);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Send a password-reset email to the specified user.
        // ..api/carer/reset?id=
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Reset([FromQuery(Name = "id")] string id)
        {
            try
            {
                bool carerExists = DbService.GetCarerById(_ctx, id).GetAwaiter().GetResult() != null;
                if (carerExists)
                {
                    bool success = DbService.SendPasswordReset(_ctx, id);
                    if (success)
                    {
                        return Ok("Password reset sent successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to send password reset.");
                    }
                }
                else
                {
                    return NotFound("Could not find a carer with that email.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Delete a user from the system, given their id.
        // ..api/carer/delete?id=
        [Authorize(Roles = "admin")]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool carerExists = DbService.GetCarerById(_ctx, id).GetAwaiter().GetResult() != null;
                if (carerExists)
                {
                    bool notDeletingSelf = !DbService.MatchCarerApiKeyWithId(_ctx, apiKey, id).GetAwaiter().GetResult();
                    if (notDeletingSelf)
                    {
                        bool success = DbService.DeleteCarer(_ctx, id).GetAwaiter().GetResult();
                        if (success)
                        {
                            return Ok("Carer deleted successfully.");
                        }
                        else
                        {
                            return BadRequest("Failed to delete carer.");
                        }
                    }
                    else
                    {
                        return Unauthorized("Accounts must be deleted by an admin other than yourself.");
                    }
                }
                else
                {
                    return NotFound("Could not find a carer with that email.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Assign a patient to a carer as specified in the body.
        // ..api/carer/assign
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Assign([FromBody] string patientAndCarerId)
        {
            return Ok("Endpoint works.");
        }


        // Unassign a patient from a carer as specified in the body.
        // ..api/carer/unassign
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Unassign([FromBody] string patientAndCarerId)
        {
            return Ok("Endpoint works.");
        }


        // Change the role/permissions of the user specified in the body.
        // ..api/carer/role
        [HttpPut]
        [Authorize(Roles = "admin")]
        public IActionResult Role([FromBody] string carerAndRole)
        {
            return Ok("Endpoint works.");
        }
    }
}
