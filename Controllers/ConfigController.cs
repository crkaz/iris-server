using System;
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
        public ConfigController(DbCtx context) : base(context) { }


        // Modify a parameter of the patient's device configuration.
        // ..api/config/put?id=
        [Authorize(Roles = _roles.carer)]
        public IActionResult Put([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromBody] JObject configJson)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    bool success = DbService.UpdatePatientConfig(_ctx, id, configJson).GetAwaiter().GetResult();
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
        [Authorize(Roles = _roles.carer)]
        public IActionResult Get([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = (Patient)DbService.GetEntityByPrimaryKey(_ctx, id, DbService.Collection.patients).GetAwaiter().GetResult();
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