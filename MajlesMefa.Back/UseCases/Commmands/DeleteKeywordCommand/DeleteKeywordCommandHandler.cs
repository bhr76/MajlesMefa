using MediatR;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteKeywordCommand
{
    public class DeleteKeywordCommandHandler : IRequestHandler<DeleteKeywordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteKeywordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteKeywordCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.KeywordRepository.DeleteAsync(request.Id);
        }
    }
}
