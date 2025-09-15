using MediatR;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteCityCommand
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var hasUser = await _unitOfWork.CityRepository.HasUserAsync(request.Id);
            if(hasUser)
            {
                throw new InvalidOperationException("ابتدا کاربران متصل را تعیین تکلیف کنید");
            }
            _unitOfWork.CityRepository.Delete(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
