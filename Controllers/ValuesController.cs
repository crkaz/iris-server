using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// BOILERPLATE CONTROLLER PROVIDED ON PROJECT CREATION
/// </summary>
namespace iris_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public ValuesController(Models.DatabaseContext context) : base(context) { }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "connected", "to", "local", "API", "http://localhost:54268/api/values" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return id.ToString();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            Console.WriteLine("Endpoint works");
            return Ok("Post ok");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            Console.WriteLine("Endpoint works");
            return Ok("Put ok");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public Boolean Delete(int id, [FromHeader(Name = "apiKey")] string apiKey)
        {
            Console.WriteLine("Endpoint works");
            return true;
        }
    }
}
