using System;
using System.Collections.Generic;
using System.Linq;
using iris_server.Models;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iris_server.Controllers
{
    public class PatientController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientController(DbCtx context) : base(context) { }


        // Delete a user from the system, given their id.
        /// ..api/patient/delete?id=
        [HttpDelete]
        [Authorize(Roles = _roles.admin)]
        public IActionResult Delete([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string patientId)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, patientId);
                if (patientAssignedToThisCarer)
                {
                    bool success = DbService.DeleteEntityByPrimaryKey(_ctx, patientId, DbService.Collection.patients).GetAwaiter().GetResult();
                    if (success)
                    {
                        return Ok();
                    }
                    return BadRequest();
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


        // Return a collection patients specified by their ids in an array in the URI.
        /// ..api/patient/list?id=..&id=
        [HttpGet]
        [Authorize(Roles = _roles.carer)]
        public IActionResult List([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string[] patientIds)
        {
            try
            {
                List<Patient> patients = new List<Patient>();

                foreach (string id in patientIds)
                {
                    bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, id);
                    if (patientAssignedToThisCarer)
                    {
                        Patient patient = (Patient)DbService.GetEntityByPrimaryKey(_ctx, id, DbService.Collection.patients).GetAwaiter().GetResult();
                        if (patient != null)
                        {
                            patients.Add(patient);
                        }
                        else
                        {
                            return BadRequest("One or more invalid patient id.");
                        }
                    }
                    else
                    {
                        return Unauthorized("You are not assigned to all of the specified patients.");
                    }
                }

                var patientsJson = JsonConvert.SerializeObject(patients);
                return Ok(patientsJson);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Get the status of a patient specified by their id.
        /// ..api/patient/status?id=
        [HttpGet]
        [Authorize(Roles = _roles.carer)]
        public IActionResult Status([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string patientId)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, patientId);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = (Patient)DbService.GetEntityByPrimaryKey(_ctx, patientId, DbService.Collection.patients).GetAwaiter().GetResult();
                    bool patientExists = patient != null;

                    if (patientExists)
                    {
                        return Ok(patient.Status);
                    }
                    else
                    {
                        return BadRequest("Patient does not exist.");
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


        // Update the status of the patient (e.g. online, offline, alert).
        /// ..api/patient/status?id=&=status
        [HttpPut]
        [Authorize(Roles = _roles.patient)]
        public IActionResult Status([FromHeader(Name = "ApiKey")] string patientApiKey, [FromQuery(Name = "id")] string patientId, [FromQuery(Name = "status")] string status)
        {
            try
            {
                Patient patient = (Patient)DbService.GetEntityByForeignKey(_ctx, patientApiKey, DbService.Collection.patients);
                if (patient.Id == patientId)
                {
                    string[] enumNames = Enum.GetNames(typeof(Patient.PatientStatus));
                    bool validStatus = enumNames.Contains(status);
                    if (validStatus)
                    {
                        patient.Status = status;
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest("Invalid status argument.");
                    }
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


        // Get the paginated logs of a patient.
        // ..api/patient/logs/?id=..&page=..&nitems=
        [HttpGet]
        [Authorize(Roles = _roles.carer)]
        public IActionResult Logs([FromHeader(Name = "ApiKey")] string carerApiKey, [FromQuery(Name = "id")] string patientId, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            try
            {
                bool patientAssignedToCarer = DbService.PatientIsAssigned(_ctx, carerApiKey, patientId);

                if (patientAssignedToCarer)
                {
                    ICollection<ActivityLog> logs = DbService.GetLogs(_ctx, patientId, page, nItems).GetAwaiter().GetResult();
                    string logsJson = JsonConvert.SerializeObject(logs);
                    return Ok(logsJson);
                }
                else
                {
                    return Unauthorized("You are not assigned to this patient.");
                }
                // Return logs as a paginated json collection
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Create a new activity log.
        // ..api/patient/logs/
        [HttpPost]
        [Authorize(Roles = _roles.patient)]
        public IActionResult Logs([FromHeader(Name = "ApiKey")] string patientApiKey, [FromBody] JObject logJson)
        {
            try
            {
                bool success = DbService.CreatePatientActivityLog(_ctx, patientApiKey, logJson).GetAwaiter().GetResult();
                if (success)
                {
                    return Ok("Logged successfully.");
                }
                else
                {
                    return BadRequest("Logging failed.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
