using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iris_server.Models;
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
        [Authorize(Roles = "admin,formalcarer,informalcarer")]
        public IActionResult Delete([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    bool success = PatientDatabaseAccess.DeletePatientById(_ctx, id);
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
        public IActionResult List([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string[] ids)
        {
            try
            {
                List<Patient> patients = new List<Patient>();

                foreach (string id in ids)
                {
                    bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                    if (patientAssignedToThisCarer)
                    {
                        Patient patient = PatientDatabaseAccess.GetPatientById(_ctx, id);

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
        public IActionResult Status([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id)
        {
            try
            {
                bool patientAssignedToThisCarer = CarerDatabaseAccess.PatientIsAssigned(_ctx, apiKey, id);
                if (patientAssignedToThisCarer)
                {
                    Patient patient = PatientDatabaseAccess.GetPatientById(_ctx, id);
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
        public IActionResult Status([FromHeader(Name = "ApiKey")]string apiKey, [FromQuery(Name = "id")] string id, [FromQuery(Name = "status")] string status)
        {
            try
            {
                bool authorised = PatientDatabaseAccess.GetPatientByApiKey(_ctx, apiKey).Id == id;
                if (authorised)
                {
                    string[] enumNames = Enum.GetNames(typeof(Patient.PatientStatus));
                    bool validStatus = enumNames.Contains(status);
                    Patient patient = PatientDatabaseAccess.GetPatientByApiKey(_ctx, apiKey);
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
    }
}