using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using System;
using Newtonsoft.Json;
using iris_server.Services;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace iris_server.Controllers
{
    public class CarerController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public CarerController(DbCtx context) : base(context) { }


        // Create a new user in the system.
        // ..api/carer/post
        [Authorize(Roles = _roles.admin)]
        public IActionResult Post([FromBody] string email)
        {
            try
            {
                bool emailValid = (!string.IsNullOrWhiteSpace(email) && email.Contains("@"));
                if (!emailValid)
                {
                    return BadRequest("Invalid email format.");
                }
                Carer carer = (Carer)DbService.GetEntityByPrimaryKey(_ctx, email, DbService.Collection.carers).GetAwaiter().GetResult();
                bool emailAlreadyExists = carer != null;
                if (emailAlreadyExists)
                {
                    return BadRequest("Email address already in use.");
                }
                bool success = DbService.CreateUser(_ctx, email, Models.User.UserRole.informalcarer).GetAwaiter().GetResult();
                if (success)
                {
                    return Ok("New carer added successfully.");
                }
                return BadRequest("Failed to create new carer.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // Return the collection of users registered in the system.
        // ..api/carer/get
        [HttpGet]
        [Authorize(Roles = _roles.admin)]
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


        #region REMOVED: Password reset endpoint: changed to use firebase library in frontend instead.
        //// Send a password-reset email to the specified user.
        //// ..api/carer/reset?id=
        //[HttpGet]
        //[Authorize(Roles = _roles.admin)]
        //public IActionResult Reset([FromQuery(Name = "id")] string email)
        //{
        //    try
        //    {
        //        Carer carer = (Carer)DbService.GetEntityByPrimaryKey(_ctx, email, DbService.Collection.carers).GetAwaiter().GetResult();
        //        bool carerExists = carer != null;
        //        if (carerExists)
        //        {
        //            bool success = carer.SendPasswordReset();
        //            if (success)
        //            {
        //                return Ok("Password reset sent successfully");
        //            }
        //            else
        //            {
        //                return BadRequest("Failed to send password reset.");
        //            }
        //        }
        //        else
        //        {
        //            return NotFound("Could not find a carer with that email.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //}
        #endregion


        // Delete a user from the system, given their id.
        // ..api/carer/delete?id=
        [Authorize(Roles = _roles.admin)]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string email)
        {
            try
            {
                Carer carer = (Carer)DbService.GetEntityByPrimaryKey(_ctx, email, DbService.Collection.carers).GetAwaiter().GetResult();
                bool carerExists = carer != null;
                if (carerExists)
                {
                    bool notDeletingSelf = carer.User.ApiKey != apiKey;
                    if (notDeletingSelf)
                    {
                        bool success = DbService.DeleteEntityByPrimaryKey(_ctx, email, DbService.Collection.carers).GetAwaiter().GetResult();
                        if (success)
                        {
                            return Ok("Carer deleted successfully.");
                        }
                        return BadRequest("Failed to delete carer.");
                    }
                    return Unauthorized("Accounts must be deleted by an admin other than yourself.");
                }
                return NotFound("Could not find a carer with that email.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Assign/unassign a patient to/from a carer as specified in the body.
        // ..api/carer/allocate
        [HttpPut]
        [Authorize(Roles = _roles.admin)]
        public IActionResult Allocate([FromBody] JObject patientAndCarerId)
        {
            try
            {
                string patientId = (string)patientAndCarerId["patient"];
                string carerEmail = (string)patientAndCarerId["carer"];
                bool assign = (bool)patientAndCarerId["assign"]; // Whether to assign or unassign.
                Patient patient = (Patient)DbService.GetEntityByPrimaryKey(_ctx, patientId, DbService.Collection.patients).GetAwaiter().GetResult();
                Carer carer = (Carer)DbService.GetEntityByPrimaryKey(_ctx, carerEmail, DbService.Collection.carers).GetAwaiter().GetResult();

                if (carer != null && patient != null)
                {
                    bool success = DbService.AllocatePatient(_ctx, patientAndCarerId).GetAwaiter().GetResult();
                    if (success)
                    {
                        if (assign)
                            return Ok("Assigned successfully.");
                        return Ok("Unassigned successfully.");
                    }
                    return BadRequest("Failed.");
                }
                return NotFound("Either the patient or carer does not exist.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Change the role/permissions of the carer as specified in the body.
        // ..api/carer/role
        [HttpPut]
        [Authorize(Roles = _roles.admin)]
        public IActionResult Role([FromBody] JObject carerAndRole)
        {
            try
            {
                string carerEmail = (string)carerAndRole["carer"];
                string roleStr = (string)carerAndRole["role"];
                Carer carer = (Carer)DbService.GetEntityByPrimaryKey(_ctx, carerEmail, DbService.Collection.carers).GetAwaiter().GetResult();
                if (carer != null)
                {
                    bool roleExists = Enum.GetNames(typeof(User.UserRole)).Contains(roleStr); ;
                    if (roleExists)
                    {
                        DbService.ChangeCarerPermission(_ctx, carerAndRole).GetAwaiter().GetResult();
                        return Ok("Role changed successfully.");
                    }
                    return NotFound("Invalid role specified.");
                }
                return NotFound("Carer does not exist.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
