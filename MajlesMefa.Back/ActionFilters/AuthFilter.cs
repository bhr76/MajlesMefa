using Azure;
using IdentityModel;
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
    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute(params RoleTypeEnum[] roles ) : base(typeof(AuthFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthFilter : IAuthorizationFilter
    {
        private readonly List<RoleTypeEnum> _roles;

        public AuthFilter(params RoleTypeEnum[] roles)
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

            var accessToken = context.HttpContext.Request.Cookies
                ["AccessToken"];
            var refreshToken = context.HttpContext.Request.Cookies
                ["RefreshToken"];
            var baseUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.PathBase}";
            
            
            if (!baseUrl.EndsWith('/'))
            {
                baseUrl += "/";
            }
            if (context.HttpContext.Request.Cookies["AspSessionToken"] == null)
            {
                if (accessToken != null || refreshToken != null)
                {
                    context.HttpContext.Response.Cookies.Delete("AccessToken");
                    context.HttpContext.Response.Cookies.Delete("RefreshToken");
                }

                context.Result = new RedirectResult(baseUrl + $"Login/Index");
            }
            if (accessToken == null && refreshToken != null)
            {
                context.Result = new RedirectResult(baseUrl+ $"Login/LoginByRefreshToken?redirectUrl={HttpUtility.UrlEncode(context.HttpContext.Request.GetEncodedUrl())}");
            }
            else if(accessToken == null && refreshToken == null)
            {
                context.Result = new RedirectResult(baseUrl + $"Login/Index");
            }
            else
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));
                if(_roles == null || _roles.Count == 0)
                {
                    return;
                }

                var hasRole = _roles?.Any(x => context.HttpContext.User.IsInRole(x.ToString())) ?? false;
                if (!hasRole)
                {
                    context.Result = new ForbidResult("نقش شما اجازه ی دسترسی ندارد");
                }
            }
        }
    }
}
