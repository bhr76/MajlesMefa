using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class PageRepository : BaseRepository<PageEntity>, IPageRepository
    {
        public PageRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public void AddRole(Guid pageId, Guid roleId)
        {
            _context.PageRoles.Add(new PageRoleEntity()
            {
                PageId = pageId,
                RoleId = roleId
            }); 
        }

        public void AddRole(PageEntity page, Guid roleId)
        {
            _context.PageRoles.Add(new PageRoleEntity()
            {
                Page = page,
                RoleId = roleId
            });
        }

        public async Task RemoveRoleAsync(Guid pageId, Guid roleId)
        {

            var entity = await _context.PageRoles.SingleOrDefaultAsync(x => x.PageId == pageId && x.RoleId == roleId);
            _context.PageRoles.Remove(entity);
        }
    }
}
