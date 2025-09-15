using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteActionReferenceCommand
{
    public class DeleteActionReferenceCommandHandler : IRequestHandler<DeleteActionReferenceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteActionReferenceCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteActionReferenceCommand request, CancellationToken cancellationToken)
        {
            
            var entity = await _unitOfWork.ActionReferenceRepository
                .FindIfEditableAsync(request.Id, _currentUserService.GetCurrentUser().UserId, _currentUserService.GetCurrentUser().Roles);
            _unitOfWork.ActionReferenceRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
