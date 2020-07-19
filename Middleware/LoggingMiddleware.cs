using iris_server.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using iris_server.Services;
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

        public async Task InvokeAsync(HttpContext context, DatabaseContext ctx)
        {
            string apiKey = context.Request.Headers["ApiKey"];
            string requestedEndpoint = context.Request.Path;
            string ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            int responseCode = context.Response.StatusCode;
            DbLog log = new DbLog(requestedEndpoint, context.Response.StatusCode, ip);

            // Log the request.
            User user = ctx.Users.Find(apiKey);
            if (user != null)
            {
                // Log request against a registered user.
                ctx.Users.Find(apiKey).DbLogs.Add(log);

                // Create a separate "activty log" for patients.
                Patient patient = (Patient)DbService.GetEntityByForiegnKey(ctx, apiKey, DbService.Collection.patients);
                bool isAPatientRequest = patient != null;
                if (isAPatientRequest)
                {
                    await DbService.LogActivity(ctx, context, patient);
                }
            }
            else
            {
                // Log request from Unrecognised user.
                string guid = Guid.NewGuid().ToString();
                log.Id = "unknown-" + guid;
                ctx.DbLogs.Add(log);
            }
            ctx.SaveChanges();

            await _next(context);
        }

    }
}
