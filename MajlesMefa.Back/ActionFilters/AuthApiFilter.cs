using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MajlesMefa.Back.ActionFilters
{
    public class AuthApiAttribute : TypeFilterAttribute
    {
        public AuthApiAttribute(params RoleTypeEnum[] roles ) : base(typeof(AuthApiFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthApiFilter : IAuthorizationFilter
    {
        private readonly List<RoleTypeEnum> _roles;

        public AuthApiFilter(params RoleTypeEnum[] roles)
        {
            _roles = roles.ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            if(hasAllowAnonymous)
            {
                return;
            }

            // Try to get token from Authorization header (Bearer token)
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "Authorization token is required",
                    StatusCode = 401
                });
                return;
            }

            try
            {
                // Validate and read the token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var validTo = jwtToken.ValidTo;

                // تبدیل زمان انقضا به منطقه زمانی محلی
                TimeZoneInfo localZone = TimeZoneInfo.Local;
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(validTo, localZone);
                if (localTime < DateTime.Now)
                {
                    context.Result = new UnauthorizedObjectResult(new
                    {
                        Message = "Invalid or expired token",
                        StatusCode = 401
                    });
                    return;
                }
                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims));

                // Role validation if roles are specified
                if (_roles.Any())
                {
                    var hasRole = _roles.Any(x => context.HttpContext.User.IsInRole(x.ToString()));
                    if (!hasRole)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "Invalid or expired token",
                    StatusCode = 401
                });
            }

          
        }
    }
}
