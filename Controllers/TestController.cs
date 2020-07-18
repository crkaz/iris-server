using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

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
    }
}