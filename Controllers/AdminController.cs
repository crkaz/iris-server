using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iris_server.Models;

namespace iris_server.Controllers
{
    public class AdminController : BaseController
    {
        /// Constructor injects the user context using dependency injection, via the BaseController. 
        public AdminController(DatabaseContext context) : base(context) { }
    }
}