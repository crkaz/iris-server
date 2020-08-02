using iris_server.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace iris_server.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Main code middleware code.
        public async Task InvokeAsync(HttpContext httpContext, DbCtx ctx)
        {
            string apiKey = httpContext.Request.Headers["ApiKey"];
            string requestedEndpoint = httpContext.Request.Path;
            DbLog log = new DbLog(requestedEndpoint);

            // Log the request.
            User user = ctx.Users.Find(apiKey);
            if (user != null)
            {
                // Log request against a registered user.
                user.DbLogs.Add(log);
            }
            else
            {
                // Log request from unrecognised users.
                string guid = Guid.NewGuid().ToString();
                log.Id = "unknown-" + guid;
                ctx.DbLogs.Add(log);
            }
            ctx.SaveChanges();

            await _next(httpContext);
        }
    }
}
