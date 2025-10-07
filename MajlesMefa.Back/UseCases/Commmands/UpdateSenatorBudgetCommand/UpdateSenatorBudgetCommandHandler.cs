using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MajlesMefa.Back.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateSenatorBudgetCommand
{
    public class UpdateSenatorBudgetCommandHandler : IRequestHandler<UpdateSenatorBudgetCommand, UpdateSenatorBudgetResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSenatorBudgetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateSenatorBudgetResult> Handle(UpdateSenatorBudgetCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.SenatorBudgetRepository.FindAsync(request.Id);
            if (entity == null)
            {
                return UpdateSenatorBudgetResult.Fail("بودجه موردنظر یافت نشد.");
            }

            var hasLoans = await _unitOfWork.SenatorBudgetRepository
                .NoTracking
                .Include(x => x.LoanRelatedBanks)
                .AnyAsync(x => x.Id == request.Id && x.LoanRelatedBanks.Any(), cancellationToken);

            if (hasLoans)
            {
                return UpdateSenatorBudgetResult.Fail("این ردیف در درخواست تسهیلات استفاده شده است و قابل ویرایش نیست.");
            }

            entity.SenatorId = request.SenatorId;
            entity.UserId = request.UserId;
            entity.Amount = request.Amount;
            entity.ExecDate = request.ExecDate;
            entity.LoanType = request.LoanType;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return UpdateSenatorBudgetResult.Success("با موفقیت بروزرسانی شد.");
        }
    }
}