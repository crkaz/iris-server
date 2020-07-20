﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace iris_server.Filters
{
    public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string unauthResponse = "Unauthorized. Check ApiKey in Header is correct.";

            try
            {
                AuthorizeAttribute authAttribute = (AuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.Where(e => e.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();

                if (authAttribute != null)
                {
                    string[] roles = authAttribute.Roles.Split(',');
                    foreach (string role in roles)
                    {
                        if (context.HttpContext.User.IsInRole(role))
                        {
                            // OK if the user claimed role is one of those specified in roles.
                            return;
                        }
                    }
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(unauthResponse);
            }
        }
    }
}
