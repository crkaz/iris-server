using iris_server.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iris_server.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserContext dbContext)
        {
            //const string apiKeyHeader = "ApiKey";
            //string apiKey = string.Empty;
            //if (context.Request.Headers.TryGetValue(apiKeyHeader, out var headerValues))
            //{
            //    apiKey = headerValues.FirstOrDefault(); // Extract from headerValues array.
            //    bool keyExists = UserDatabaseAccess.LookupApiKey(dbContext, apiKey);

            //    if (keyExists)
            //    {
            //        User user = UserDatabaseAccess.GetUserByApiKey(dbContext, apiKey);

            //        Claim[] claims =
            //        {
            //            new Claim(ClaimTypes.Name, user.UserName),
            //            new Claim(ClaimTypes.Role, user.Role)
            //        };

            //        ClaimsIdentity identity = new ClaimsIdentity(claims, "ApiKey");
            //        context.User.AddIdentity(identity);
            //    }

            //}

            await _next(context);
        }

    }
}
