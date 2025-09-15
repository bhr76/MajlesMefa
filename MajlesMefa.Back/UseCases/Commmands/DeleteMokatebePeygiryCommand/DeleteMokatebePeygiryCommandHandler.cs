using MediatR;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteMokatebePeygiryCommand
{
    public class DeleteMokatebePeygiryCommandHandler : IRequestHandler<DeleteMokatebePeygiryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMokatebePeygiryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteMokatebePeygiryCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.PeygiriRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
