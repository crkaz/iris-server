using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Newtonsoft.Json.Linq;
using iris_server.Services;

namespace iris_server.Controllers
{
    public class ComputeController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ComputeController(DatabaseContext context) : base(context) { }

        // Analyse an image and return the predicted room and context aware prompt.
        // ..api/compute/detectroom
        [HttpGet]
        // [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DetectRoom([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")]string id)
        {
            try
            {
                bool authorised = await DbService.MatchApiKeyWithId(_ctx, apiKey, id);
                if (authorised)
                {
                    //byte[] imageBytes = await Request.GetRawBodyBytesAsync();
                    //// Get azure labels
                    //// Pass to detection service
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Analyse an image and return annotations.
        // ..api/compute/detectconfusion
        [HttpGet]
        // [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DetectConfusion([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")]string id)
        {
            try
            {
                bool authorised = await DbService.MatchApiKeyWithId(_ctx, apiKey, id);
                if (authorised)
                {
                    //byte[] imageBytes = await Request.GetRawBodyBytesAsync();
                    //// Get azure labels
                    //// Pass to detection service
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // Analyse an array of collection of transforms and return whether or not a fall occurred..
        // ..api/compute/detectfall
        [HttpGet]
        // [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DetectFall([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery(Name = "id")]string id, [FromBody] JObject transforms)
        {
            //return DetectionService.DetectFall(transforms);

            try
            {
                bool authorised = await DbService.MatchApiKeyWithId(_ctx, apiKey, id);
                if (authorised)
                {
                    //byte[] imageBytes = await Request.GetRawBodyBytesAsync();
                    //// Get azure labels
                    //// Pass to detection service
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
