using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class ActionReferenceRepository : BaseRepository<ActionReferenceEntity>, IActionReferenceRepository
    {
        public ActionReferenceRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public async Task<ActionReferenceEntity> FindIfEditableAsync(Guid id, Guid currentUserId, List<RoleTypeEnum> roles)
        {
            var entity = await _context.ActionReferences
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

         
            bool haveAccessToDelete = roles.Any(x=> x.HasFlag(RoleTypeEnum.Admin) || x.HasFlag(RoleTypeEnum.MinistryAdmin) || x.HasFlag(RoleTypeEnum.MinistryMember));

            if (!haveAccessToDelete)
            {
                if (entity.FromUserId != currentUserId)
                {
                    throw new InvalidOperationException("اجازه تغییر اقدام کاربر دیگر را ندارید");
                }
            }
           

            var lastActionId = await _context.ActionReferences
                .Where(x => x.DataEntryId == entity.DataEntryId)
                .OrderByDescending(x => x.Created)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (lastActionId != id)
            {
                throw new InvalidOperationException("تنها امکان تغییر آخرین اقدام وجود دارد، برای تغییر ابتدا باید اقدامات بعدی حذف شود");
            }

            return entity;
        }

    }
}
