using System;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;
using iris_server.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using iris_server.Extensions;
using System.Threading.Tasks;

namespace iris_server.Controllers
{
    public class ComputeController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ComputeController(DbCtx context) : base(context) { }


        // Amalyses a collection of transforms (position and rotation) and returns a dictionary of functionalities and results (e.g. "falldetection": true").
        public IDictionary<string, object> AnalyseMovement([FromHeader(Name = "ApiKey")] string apiKey, [FromBody] JObject transformsJson)
        {
            try
            {
                IDictionary<string, object> result = DetectionService.AnalyseMovement(transformsJson);
                return result;
            }
            catch
            {
                return null;
            }
        }


        // Analyse an image and return the json response.
        // ..api/compute/analyseimage
        [HttpPost]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> AnalyseImage([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                byte[] imageBytes = await Request.GetRawBodyBytesAsync();
                var response = await AzureVisionService.Analyse(imageBytes);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // Analyse an image and return the predicted room and context aware prompt.
        // ..api/compute/detectroom
        [HttpPost]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> DetectRoom([FromHeader(Name = "ApiKey")] string apiKey)
        {
            try
            {
                byte[] imageBytes = await Request.GetRawBodyBytesAsync();
                var response = await DetectionService.DetectRoom(imageBytes);
                return Ok(response);
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
            //bool result = DetectionService.DetectFall(transforms);
            //return result;
            return true;
        }
    }
}
