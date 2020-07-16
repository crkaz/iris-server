using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iris_server.Extensions;
using iris_server.Services;
using iris_server.Models;

namespace iris_server.Controllers
{
    public class ComputeController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ComputeController(DatabaseContext context) : base(context) { }

        // Analyse an image and return the predicted room and context aware prompt.
        // ..api/feature/detectroom
        [HttpGet]
        // [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DetectRoom([FromHeader(Name = "ApiKey")] string apiKey)
        {
            byte[] imageBytes = await Request.GetRawBodyBytesAsync();
            return Ok("Endpoint works.");
        }


        // Analyse an image and return annotations.
        // ..api/feature/detectconfusion
        [HttpGet]
        // [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DetectConfusion([FromHeader(Name = "ApiKey")] string apiKey)
        {
            byte[] imageBytes = await Request.GetRawBodyBytesAsync();
            return Ok("Endpoint works.");
        }


        // Analyse an array of collection of transforms and return whether or not a fall occurred..
        // ..api/feature/detectfall
        [HttpGet]
        // [Authorize(Roles = "Patient")]
        public bool DetectFall([FromBody] string transforms)
        {
            return DetectionService.DetectFall(transforms);
        }
    }
}
