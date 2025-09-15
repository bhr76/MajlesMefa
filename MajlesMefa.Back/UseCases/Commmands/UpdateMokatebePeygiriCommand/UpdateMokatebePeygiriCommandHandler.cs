using AutoMapper;
using MediatR;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateMokatebePeygiriCommand
{
    public class UpdateMokatebePeygiriCommandHandler : IRequestHandler<UpdateMokatebePeygiriCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateMokatebePeygiriCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateMokatebePeygiriCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.PeygiriRepository.FindAsync(request.Id);
            entity = _mapper.Map(request, entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
