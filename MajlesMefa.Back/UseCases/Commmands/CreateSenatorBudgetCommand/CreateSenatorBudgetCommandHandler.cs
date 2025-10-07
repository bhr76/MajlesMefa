using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;

namespace MajlesMefa.Back.UseCases.Commmands.CreateSenatorBudgetCommand
{
    public class CreateSenatorBudgetCommandHandler : IRequestHandler<CreateSenatorBudgetCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSenatorBudgetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSenatorBudgetCommand request, CancellationToken cancellationToken)
        {
            var entity = new SenatorBudgetEntity
            {
                Id = Guid.NewGuid(),
                SenatorId = request.SenatorId,
                UserId = request.UserId,
                Amount = request.Amount,
                ExecDate = request.ExecDate,
                RegisterDate = DateTime.UtcNow,
                LoanType = request.LoanType
            };

            _unitOfWork.SenatorBudgetRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}