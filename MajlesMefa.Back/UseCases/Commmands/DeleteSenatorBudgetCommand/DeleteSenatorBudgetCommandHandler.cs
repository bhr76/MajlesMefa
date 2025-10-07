using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MajlesMefa.Back.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteSenatorBudgetCommand
{
    public class DeleteSenatorBudgetCommandHandler : IRequestHandler<DeleteSenatorBudgetCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSenatorBudgetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSenatorBudgetCommand request, CancellationToken cancellationToken)
        {
            var hasLoans = await _unitOfWork.SenatorBudgetRepository
                .NoTracking
                .Include(x => x.LoanRelatedBanks)
                .AnyAsync(x => x.Id == request.Id && x.LoanRelatedBanks.Any(), cancellationToken);

            if (hasLoans)
            {
                throw new InvalidOperationException("این ردیف در درخواست تسهیلات استفاده شده است و قابل حذف نیست.");
            }

            await _unitOfWork.SenatorBudgetRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}