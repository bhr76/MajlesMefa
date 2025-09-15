using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface IActionReferenceRepository: IRepository<ActionReferenceEntity>
    {
        Task<ActionReferenceEntity> FindIfEditableAsync(Guid id, Guid currentUserId, List<RoleTypeEnum> roles);
    }
}
