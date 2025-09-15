using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.CreateMokatebePeygiriCommand
{
    public class CreateMokatebePeygiriCommandHandler : IRequestHandler<CreateMokatebePeygiriCommand, long>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMokatebePeygiriCommandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateMokatebePeygiriCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.DataEntryRepository.CheckIsClosedAsync(request.DataEntryId);
            var entity = _mapper.Map<PeygiriEntity>(request);
            _unitOfWork.PeygiriRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
