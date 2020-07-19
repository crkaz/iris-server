using iris_server.Models;
using iris_server.Services;
using Microsoft.AspNetCore.Http;
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

        public async Task InvokeAsync(HttpContext httpContext, DatabaseContext ctx)
        {
            const string expectedApiKeyHeader = "ApiKey";
            string apiKey = httpContext.Request.Headers[expectedApiKeyHeader];
            if (apiKey != null)
            {
                User user = (User)await DbService.GetEntityByPrimaryKey(ctx, apiKey, DbService.Collection.users);
                if (user != null)
                {
                    Claim[] claims =
                    {
                        new Claim(ClaimTypes.Name, user.ApiKey),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "ApiKey");
                    httpContext.User.AddIdentity(identity);
                }
            }
            await _next(httpContext);
        }
    }
}
