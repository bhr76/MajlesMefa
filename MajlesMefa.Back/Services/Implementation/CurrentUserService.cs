using IdentityContext.Dtos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Services.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public CurrentUserDto GetCurrentUser()
        {
            var rslt = new CurrentUserDto();
            if(Guid.TryParse(_contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid guserid))
            {
                rslt.UserId = guserid;

                rslt.IpAddress = _contextAccessor != null && _contextAccessor.HttpContext != null &&
                _contextAccessor.HttpContext.Request.Headers.Any(o => o.Key == "X-Forwarded-For") ?
                _contextAccessor.HttpContext?.Request.Headers.First(o => o.Key == "X-Forwarded-For").Value.ToString() :
                _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                rslt.UserName = _contextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;
                rslt.Name = _contextAccessor.HttpContext?.User?.FindFirst("Fullname")?.Value;
                rslt.BussinessUserId = Guid.Parse(_contextAccessor.HttpContext?.User?.FindFirst("BussinessUserId")?.Value);
                rslt.Roles = _contextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.Role)
                    ?.Value
                    ?.Split(",")
                    .Select(s => Enum.Parse<RoleTypeEnum>(s))
                    ?.ToList();
                rslt.RoleNames = _contextAccessor.HttpContext?.User?
                    .FindFirst("RoleNames")
                    ?.Value
                    ?.Split(",")
                    ?.ToList();
                var hasCity = _contextAccessor.HttpContext?.User?
                    .FindFirst("CityId") != null;
                var hasOrganization = _contextAccessor.HttpContext?.User?
                    .FindFirst("OrganizationId") != null;
                if(hasCity)
                {

                    rslt.City.Id = Guid.Parse(_contextAccessor.HttpContext?.User?
                        .FindFirst("CityId")
                        ?.Value);
                    rslt.City.Name =
                        _contextAccessor.HttpContext?.User?
                        .FindFirst("CityName")
                        ?.Value;
                    var hasParentCity = _contextAccessor.HttpContext?.User?
                    .FindFirst("CityParentId") != null;
                    if(hasParentCity)
                    {
                        rslt.City.ParentId =
                            Guid.Parse(_contextAccessor.HttpContext?.User?
                            .FindFirst("CityParentId")
                            ?.Value);
                        rslt.City.ParentName = _contextAccessor.HttpContext?.User?
                            .FindFirst("CityParentName")
                            ?.Value;
                    }
                }
                if(hasOrganization)
                {

                    rslt.Organization.Id = Guid.Parse(_contextAccessor.HttpContext?.User?
                        .FindFirst("OrganizationId")
                        ?.Value);
                    rslt.Organization.Name =
                        _contextAccessor.HttpContext?.User?
                        .FindFirst("OrganizationName")
                        ?.Value;
                    var hasParentOrganization = _contextAccessor.HttpContext?.User?
                    .FindFirst("OrganizationParentId") != null;
                    if(hasParentOrganization)
                    {
                        rslt.Organization.ParentId =
                            Guid.Parse(_contextAccessor.HttpContext?.User?
                            .FindFirst("OrganizationParentId")
                            ?.Value);
                        rslt.Organization.ParentName = _contextAccessor.HttpContext?.User?
                            .FindFirst("OrganizationParentName")
                            ?.Value;
                    }
                }
            }
            return rslt;
        }
    }
}
