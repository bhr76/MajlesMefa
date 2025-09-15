using AutoMapper;
using MediatR;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateOrganizationCommmand
{
    public class UpdateOrganizationCommmandHandler : IRequestHandler<UpdateOrganizationCommmand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOrganizationCommmandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateOrganizationCommmand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.OrganizationRepositroy.FindAsync(request.Id);
            entity = _mapper.Map(request, entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
