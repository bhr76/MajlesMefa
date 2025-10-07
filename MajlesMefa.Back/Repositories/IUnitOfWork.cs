using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories
{
    public interface IUnitOfWork
    {
        IDataEntryRepository DataEntryRepository { get; }
        IPeygiriRepository PeygiriRepository { get; }
        IActionReferenceRepository ActionReferenceRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrganizationRepositroy OrganizationRepositroy { get; }
        ICityRepository CityRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISenatorRepository SenatorRepository { get; }
        ISenatorBudgetRepository SenatorBudgetRepository { get; }
        IPageRepository PageRepository { get; }
        ITahghighTafahosRepository TahghighTafahosReposity { get; }
        ITahghighTafahosSenatorRepository TahghighTafahosSenatorReposity { get; }
        IKeywordRepository KeywordRepository { get; }

        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
