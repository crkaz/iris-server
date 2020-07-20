using Microsoft.AspNetCore.Mvc;
using iris_server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace iris_server.Controllers
{
    public class TestController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public TestController(DbCtx context) : base(context) { }


        // Check if the host is available.
        // ..api/test/status/
        [HttpGet]
        public IActionResult Status()
        {
            return Ok("iris-server is online");
        }

        // Used to test that endpoints with the patient role reject all other requests.
        [HttpGet]
        [Authorize(Roles = _roles.patient)]
        public IActionResult AuthFilterPatient()
        {
            return Ok("Patient auth works");
        }


        // Used to test that endpoints with the informalcarer role reject all other requests.
        [HttpGet]
        [Authorize(Roles = "informalcarer")]
        public IActionResult AuthFilterinFormalCarer()
        {
            return Ok("Informal carer auth works");
        }


        // Used to test that endpoints with the formalcarer role reject all other requests.
        [HttpGet]
        [Authorize(Roles = "formalcarer")]
        public IActionResult AuthFilterFormalCarer()
        {
            return Ok("Formal carer auth works");
        }


        // Used to test that endpoints with the admin role rejects all other requests.
        [HttpGet]
        [Authorize(Roles = _roles.admin)]
        public IActionResult AuthFilterAdmin()
        {
            return Ok("Admin auth works");
        }


        // Used to test that endpoints with the all/multiple roles reject all other (unknown) requests.
        [HttpGet]
        [Authorize(Roles = "admin,formalcarer,informalcarer,patient")]
        public IActionResult AuthFilterUnknown()
        {
            return Ok("All auths work");
        }


        // Used to test that logging middleware works by returning the number of dblogs.
        [HttpGet]
        [Authorize(Roles = _roles.admin)]
        public int NLogs()
        {
            return _ctx.DbLogs.Count();
        }
    }
}