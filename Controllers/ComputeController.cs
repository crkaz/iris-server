using System;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;

namespace iris_server.Controllers
{
    public class ComputeController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ComputeController(DbCtx context) : base(context) { }

        // Analyse an image and return the predicted room and context aware prompt.
        // ..api/compute/detectroom
        [HttpPost]
        // [Authorize(Roles = "Patient")]
        public IActionResult DetectRoom([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Analyse an image and return annotations.
        // ..api/compute/detectconfusion
        [HttpPost]
        // [Authorize(Roles = "Patient")]
        public IActionResult DetectConfusion([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                //byte[] imageBytes = await Request.GetRawBodyBytesAsync();
                //// Get azure labels
                //// Pass to detection service
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Analyse an array of collection of transforms and return whether or not a fall occurred..
        // ..api/compute/detectfall
        [HttpPost]
        [Authorize(Roles = _roles.patient)]
        public bool DetectFall([FromHeader(Name = "ApiKey")] string apiKey, [FromBody] JObject transforms)
        {
            bool result = DetectionService.DetectFall(transforms, null);
            return result;
        }
    }
}
