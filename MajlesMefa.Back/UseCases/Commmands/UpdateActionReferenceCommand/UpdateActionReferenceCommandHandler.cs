using AutoMapper;
using MediatR;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateActionReferenceCommand
{
    public class UpdateActionReferenceCommandHandler : IRequestHandler<UpdateActionReferenceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateActionReferenceCommandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task Handle(UpdateActionReferenceCommand request, CancellationToken cancellationToken)
        {
            var cid = _currentUserService.GetCurrentUser().UserId;
            if (!request.RefrenceUserId.HasValue)
            {
                request.RefrenceUserId = cid;
            }
            var entity = await _unitOfWork.ActionReferenceRepository
                .FindIfEditableAsync(request.Id, cid, _currentUserService.GetCurrentUser().Roles);
            entity = _mapper.Map(request, entity);
            await _unitOfWork.DataEntryRepository.CheckIsClosedAsync(entity.DataEntryId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
