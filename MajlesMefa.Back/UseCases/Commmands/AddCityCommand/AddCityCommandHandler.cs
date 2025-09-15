using AutoMapper;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddCityCommand
{
    public class AddCityCommandHandler : IRequestHandler<AddCityCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddCityCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<CityEntity>(request);
            _unitOfWork.CityRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
