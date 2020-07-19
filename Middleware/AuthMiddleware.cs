using iris_server.Models;
using iris_server.Services;
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

        public async Task InvokeAsync(HttpContext context, DatabaseContext ctx)
        {
            const string apiKeyHeader = "ApiKey";
            string apiKey = string.Empty;
            if (context.Request.Headers.TryGetValue(apiKeyHeader, out var headerValues))
            {
                apiKey = headerValues.FirstOrDefault(); // Extract from headerValues array.
                bool keyExists = await DbService.LookupPrimaryKey(ctx, apiKey, DbService.Collection.users);
                if (keyExists)
                {
                    User user = (User)await DbService.GetEntityByPrimaryKey(ctx, apiKey, DbService.Collection.users);
                    Claim[] claims =
                    {
                        new Claim(ClaimTypes.Name, user.ApiKey),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "ApiKey");
                    context.User.AddIdentity(identity);
                }

            }

            await _next(context);
        }

    }
}
