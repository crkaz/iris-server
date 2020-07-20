using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

/// <summary>
/// Base controller manages route format for all controllers and uses
/// dependency injection to inject the context into all controllers.
/// </summary>
namespace iris_server.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly DbCtx _ctx; // Controller dependency. Declared in abstract class for injection.
        protected struct _roles
        {
            public const string all = "admin,informalcarer,formalcarer,patient";
            public const string carer = "admin,informalcarer,formalcarer";
            public const string admin = "admin";
            public const string patient = "patient";
        };


        public BaseController(DbCtx context)
        {
            _ctx = context;
        }
    }
}