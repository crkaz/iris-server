using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;
using System;
using iris_server.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace iris_server.Controllers
{
    public class StickiesController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public StickiesController(DbCtx context) : base(context) { }


        // Get the json serialised sticky notes for a patient.
        // ..api/stickies/get
        [Authorize(Roles = _roles.patient)]
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                ICollection<StickyNote> entries = DbService.GetStickyNotes(_ctx, apiKey);
                string stickiesJson = JsonConvert.SerializeObject(entries);
                if (stickiesJson == "[]")
                {
                    return NotFound("Patient has no sticky notes.");
                }
                return Ok(stickiesJson);
                // Return stickies as a json collection
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Create a sticky notes for a patient.
        // ..api/stickies/post
        [Authorize(Roles = _roles.patient)]
        public IActionResult Post([FromHeader(Name = "ApiKey")] string patientApiKey, [FromBody] JObject stickyJson)
        {
            try
            {
                bool success = DbService.AddStickyNote(_ctx, patientApiKey, stickyJson).GetAwaiter().GetResult();
                if (success)
                {
                    return Ok("Successfully added sticky note.");
                }
                return BadRequest("Failed to add sticky note.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Modify a sticky note.
        // ..api/stickies/put/?id=
        [Authorize(Roles = _roles.patient)]
        public IActionResult Put([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string stickyId, [FromBody] JObject stickyJson)
        {
            try
            {
                Patient patient = (Patient)DbService.GetEntityByForeignKey(_ctx, apiKey, DbService.Collection.patients);
                bool patientOwnsSticky = patient.Stickies.Where(s => s.Id == stickyId).Count() == 1;
                if (patientOwnsSticky)
                {
                    bool success = DbService.UpdateStickyNote(_ctx, stickyId, stickyJson).GetAwaiter().GetResult();
                    if (success)
                    {
                        return Ok("Sticky note updated successfully.");
                    }
                    return BadRequest("Failed to update sticky note.");
                }
                return Unauthorized("Patient does not own that sticky note.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Delete a sticky note.
        // ..api/stickies/delete/?id=
        [Authorize(Roles = _roles.patient)]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string stickyId)
        {
            try
            {
                Patient patient = (Patient)DbService.GetEntityByForeignKey(_ctx, apiKey, DbService.Collection.patients);
                bool patientOwnsSticky = patient.Stickies.Where(s => s.Id == stickyId).Count() == 1;
                if (patientOwnsSticky)
                {
                    bool success = DbService.DeleteEntityByPrimaryKey(_ctx, stickyId, DbService.Collection.stickies).GetAwaiter().GetResult();
                    if (success)
                    {
                        return Ok("Sticky note deleted successfully.");
                    }
                    return BadRequest("Failed to delete sticky note.");
                }
                return Unauthorized("Patient does not own that sticky note.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}