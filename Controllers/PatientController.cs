using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iris_server.Models;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iris_server.Controllers
{
    public class PatientController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public PatientController(DatabaseContext context) : base(context) { }


        // Delete a user from the system, given their id.
        /// ..api/patient/delete?id=
        [HttpDelete]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult List([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string[] ids)
        {
            try
            {
                List<Patient> patients = new List<Patient>();

                foreach (string id in ids)
                {
                    bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
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
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Status([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = (Patient)DbService.GetEntityByPrimaryKey(_ctx, id, DbService.Collection.patients).GetAwaiter().GetResult();
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
        [Authorize(Roles = "patient")]
        public IActionResult Status([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "status")] string status)
        {
            try
            {
                Patient patient = (Patient)DbService.GetEntityByForiegnKey(_ctx, apiKey, DbService.Collection.patients);
                if (patient.Id == id)
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
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Logs([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "page")] string page, [FromQuery(Name = "nitems")] string nItems)
        {
            try
            {
                bool patientAssignedToCarer = DbService.PatientIsAssigned(_ctx, apiKey, id);

                if (patientAssignedToCarer)
                {
                    ICollection<ActivityLog> logs = DbService.GetLogs(_ctx, id, page, nItems).GetAwaiter().GetResult();
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
    }
}
