using MediatR;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteOrganizationCommand
{
    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrganizationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.OrganizationRepositroy.Delete(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
