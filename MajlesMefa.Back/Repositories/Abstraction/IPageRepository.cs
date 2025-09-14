using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface IPageRepository: IRepository<PageEntity>
    {
        Task RemoveRoleAsync(Guid pageId, Guid roleId);

        void AddRole(Guid pageId, Guid roleId);
        
        void AddRole(PageEntity page, Guid roleId);
    }
}
