using System;
using System.Threading.Tasks;
using iris_server.Models;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class ConfigController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ConfigController(DatabaseContext context) : base(context) { }


        // Modify a parameter of the patient's device configuration.
        // ..api/config/put?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public async Task<IActionResult> Put([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject configJson)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    bool success = await DbService.UpdatePatientConfig(_ctx, id, configJson);
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


        // Get the patient's device configuration.
        // ..api/config/get?id=
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public async Task<IActionResult> Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = await DbService.GetPatientById(_ctx, id);
                    return Ok(patient.Config);
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