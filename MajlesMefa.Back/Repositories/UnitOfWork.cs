using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using MajlesMefa.Back.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RefahMajlesDbContext _context;

        private IDataEntryRepository _dataEntryRepository;
        private IPeygiriRepository _peygiriRepository;
        private IActionReferenceRepository _actionReferenceRepository;
        private ICategoryRepository _categoryRepository;
        private IOrganizationRepositroy _organizationRepositroy;
        private ICityRepository _cityRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepositroy;
        private IPageRepository _pageRepositroy;
        private ITahghighTafahosRepository _tahghighTafahosRepositroy;
        private ITahghighTafahosSenatorRepository _tahghighTafahosSenatorRepositroy;
        private ISenatorRepository _senatorRepository;
        private IKeywordRepository _keywordRepository;

        public UnitOfWork(RefahMajlesDbContext context)
        {
            _context = context;
        }

        public IDataEntryRepository DataEntryRepository => _dataEntryRepository ??= new DataEntryRepository(_context);
        public IPeygiriRepository PeygiriRepository => _peygiriRepository ??= new PeygiriRepository(_context);
        public IActionReferenceRepository ActionReferenceRepository => _actionReferenceRepository ??= new ActionReferenceRepository(_context);
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        public IOrganizationRepositroy OrganizationRepositroy => _organizationRepositroy ??= new OrganizationRepositroy(_context);
        public ICityRepository CityRepository => _cityRepository ??= new CityRepository(_context);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public IRoleRepository RoleRepository => _roleRepositroy ??= new RoleRepository(_context);
        public IPageRepository PageRepository => _pageRepositroy ??= new PageRepository(_context);
        public ITahghighTafahosRepository TahghighTafahosReposity => _tahghighTafahosRepositroy ??= new TahghighTafahosRepository(_context);
        public ITahghighTafahosSenatorRepository TahghighTafahosSenatorReposity => _tahghighTafahosSenatorRepositroy ??= new TahghighTafahosSenatorRepository(_context);
        public ISenatorRepository SenatorRepository => _senatorRepository ??= new SenatorRepository(_context);
        public IKeywordRepository KeywordRepository => _keywordRepository ??= new KeywordRepository(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
