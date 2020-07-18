using System;
using System.Threading.Tasks;
using iris_server.Models;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class PatientInfoController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientInfoController(DatabaseContext context) : base(context) { }


        // Edit a patient's 'notes'.
        // ..api/patientinfo/put?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public async Task<IActionResult> Put([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject notesJson)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    bool success = await DbService.UpdatePatientNotes(_ctx, id, notesJson);
                    if (success)
                    {
                        return Ok("Updated patient successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to update the patient.");
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


        // Get a patient's 'notes'.
        // ..patientinfo/get/info?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public async Task<IActionResult> Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = await DbService.GetPatientById(_ctx, id);
                    return Ok(patient.Notes);
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
    }
}