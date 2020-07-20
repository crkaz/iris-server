using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

/// <summary>
/// Base controller manages route format for all controllers and enforces
/// dependency injection to inject the user context into all requests.
/// </summary>
namespace iris_server.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected struct _roles
        {
            public const string all = "admin,informalcarer,formalcarer,patient";
            public const string carer = "admin,informalcarer,formalcarer";
            public const string admin = "admin";
            public const string patient = "patient";
        };
        protected readonly DbCtx _ctx;

        public BaseController(DbCtx context)
        {
            _ctx = context;
        }
    }
}