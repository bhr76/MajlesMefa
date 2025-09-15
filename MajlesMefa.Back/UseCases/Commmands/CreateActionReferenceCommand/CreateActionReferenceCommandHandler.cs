using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand
{
    public class CreateActionReferenceCommandHandler : IRequestHandler<CreateActionReferenceCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateActionReferenceCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }


        public async Task<Guid> Handle(CreateActionReferenceCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.DataEntryRepository.CheckIsClosedAsync(request.DataEntryId);
            var cid = _currentUserService.GetCurrentUser().BussinessUserId;
            if (!request.RefrenceUserId.HasValue)
            {
                request.RefrenceUserId = cid;
            }
            var entity = _mapper.Map<ActionReferenceEntity>(request);
            entity.FromUserId = cid;
            entity.CreatorUserId = cid;
            _unitOfWork.ActionReferenceRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
