using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateCityCommand
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper= mapper;
        }

        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CityRepository.FindAsync(request.Id);
            entity = _mapper.Map(request, entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
