using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddOrganizationCommand
{
    public class AddOrganizationCommandHandler : IRequestHandler<AddOrganizationCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddOrganizationCommandHandler(IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddOrganizationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<OrganizationEntity>(request);
            _unitOfWork.OrganizationRepositroy.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
