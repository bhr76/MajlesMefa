using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Services.Implementation
{
    public class BussinessAccessControlService : IBussinessAccessControlService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly RefahMajlesDbContext _refahMajlesDbContext ;

        public BussinessAccessControlService(ICurrentUserService currentUserService, 
            RefahMajlesDbContext refahMajlesDbContext)
        {
            _currentUserService = currentUserService;
            _refahMajlesDbContext = refahMajlesDbContext;
        }

        public void CheckAccessToDataEntry(Guid dataEntryId)
        {
            var cuser = _currentUserService.GetCurrentUser();

            if (!cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin || x == RoleTypeEnum.Admin || x == RoleTypeEnum.MinistryMember ))
            {
                var hasAccess = _refahMajlesDbContext.ActionReferences
                    .Where(x => x.ToUserId == cuser.UserId
                        || x.FromUserId == cuser.UserId)
                    .Any();
                if (!hasAccess)
                {
                    throw new AccessViolationException("عدم دسترسی");
                }
            }
        }

        public async Task CheckAccessToDataEntryAsync(Guid dataEntryId)
        {
            var cuser = _currentUserService.GetCurrentUser();

            if ( !cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin))
            {
                var hasAccess = await _refahMajlesDbContext.ActionReferences
                    .Where(x => x.ToUserId == cuser.UserId
                        || x.FromUserId == cuser.UserId)
                    .AnyAsync();
                if (!hasAccess)
                {
                    throw new AccessViolationException("عدم دسترسی");
                }
            }
        }
      
    }
}
