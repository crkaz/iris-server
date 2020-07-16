﻿using Microsoft.AspNetCore.Mvc;
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
        protected readonly DatabaseContext _ctx;

        public BaseController(DatabaseContext context)
        {
            _ctx = context;
        }
    }
}