using iris_server.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using iris_server.Services;

namespace iris_server.Middleware
{
    public class LoggingMiddleware
    {
        // Set to false for production.
        const bool TEST_MODE = true;

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DatabaseContext dbContext)
        {
            const string apiKeyHeader = "ApiKey";
            string apiKey = string.Empty;
            if (context.Request.Headers.TryGetValue(apiKeyHeader, out var headerValues))
            {
                // Request response.
                apiKey = headerValues.FirstOrDefault(); // Extract from headerValues array.
                string requestedEndpoint = context.Request.Path;
                int responseCode = context.Response.StatusCode;

                // Add the log.
                DbLog log = new DbLog(requestedEndpoint, responseCode, TEST_MODE);
                dbContext.Users.Find(apiKey).DbLogs.Add(log);
                dbContext.SaveChanges();
            }

            await _next(context);
        }

    }
}
