using System;
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
        public IActionResult Put([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string patientId, [FromBody] JObject notesJson)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, patientId);
                if (patientAssignedToThisCarer)
                {
                    bool success = DbService.UpdatePatientNotes(_ctx, patientId, notesJson).GetAwaiter().GetResult();
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
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = (Patient)DbService.GetEntityByPrimaryKey(_ctx, id, DbService.Collection.patients).GetAwaiter().GetResult();
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