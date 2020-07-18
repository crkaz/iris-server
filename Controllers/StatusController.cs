using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

namespace iris_server.Controllers
{
    public class StatusController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public StatusController(DatabaseContext context) : base(context) { }


        // Check if the host is available.
        // ..api/status/
        [HttpGet]
        public IActionResult Check()
        {
            return Ok("iris-server is online");
        }
    }
}