using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Microsoft.AspNetCore.Authorization;

namespace iris_server.Controllers
{
    public class TestController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public TestController(DatabaseContext context) : base(context) { }


        // Check if the host is available.
        // ..api/test/status/
        [HttpGet]
        public IActionResult Status()
        {
            return Ok("iris-server is online");
        }


        [HttpGet]
        [Authorize(Roles = "patient")]
        public IActionResult AuthFilterPatient()
        {
            return Ok("Patient auth works");
        }


        [HttpGet]
        [Authorize(Roles = "informalcarer")]
        public IActionResult AuthFilterinFormalCarer()
        {
            return Ok("Informal carer auth works");
        }


        [HttpGet]
        [Authorize(Roles = "formalcarer")]
        public IActionResult AuthFilterFormalCarer()
        {
            return Ok("Formal carer auth works");
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AuthFilterAdmin()
        {
            return Ok("Admin auth works");
        }


        [HttpGet]
        [Authorize(Roles = "admin,formalcarer,informalcarer,patient")]
        public IActionResult AuthFilterUnknown()
        {
            return Ok("All auths work");
        }
    }
}