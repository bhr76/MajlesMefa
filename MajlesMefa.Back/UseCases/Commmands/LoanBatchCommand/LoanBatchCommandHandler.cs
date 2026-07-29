using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.UseCases.Queries.GetFlatLoanDataEntriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MajlesMefa.Back.UseCases.Commmands.LoanBatchCommand
{
    public class BatchChangeStatusCommandHandler : IRequestHandler<BatchChangeStatusCommand, bool>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly IMediator _mediator;

        public BatchChangeStatusCommandHandler(RefahMajlesDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<bool> Handle(BatchChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            List<Guid> targetLoanIds;

            if (dto.AllSelected)
            {

                if (request.Dto.Filter != null)
                {
                    DynamicQueryFilterNormalizer.NormalizeFilters(request.Dto.Filter, typeof(FlatDataEntryDto));
                }


                var query = new GetFlatLoanDataEntriesQuery
                {
                    Filter = dto.Filter,
                    SenatorId = dto.SenatorId
                };

                try
                {
                    var filteredList = await _mediator.Send(query, cancellationToken);
                    targetLoanIds = filteredList.Items.Select(x => x.Id).ToList();
                }
                catch (Exception ex)
                {

                    throw;
                }
               
            }
            else
            {
                targetLoanIds = dto.SelectedIds;
            }

            if (targetLoanIds == null || !targetLoanIds.Any())
                return false;

            // ویرایش رکوردها در دسته‌های ۵۰۰ تایی جهت بهینه‌سازی منابع پایگاه داده
            const int batchSize = 500;
            for (int i = 0; i < targetLoanIds.Count; i += batchSize)
            {
                var currentBatchIds = targetLoanIds.Skip(i).Take(batchSize).ToList();
                var dbLoans = await _context.Loans
                    .Where(x => currentBatchIds.Contains(x.DataEntryId))
                    .ToListAsync(cancellationToken);

                foreach (var loan in dbLoans)
                {
                    loan.VaziatPasokh = (Enums.ResponseStatusEnum) dto.Status;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }

    public class BatchRegisterActionCommandHandler : IRequestHandler<BatchRegisterActionCommand, bool>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public BatchRegisterActionCommandHandler(RefahMajlesDbContext context, IMediator mediator, ICurrentUserService currentUserService)
        {
            _context = context;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(BatchRegisterActionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            List<Guid> targetLoanIds;

            if (dto.AllSelected)
            {
                var query = new GetFlatLoanDataEntriesQuery
                {
                    Filter = dto.Filter,
                    SenatorId = dto.SenatorId
                };

                var filteredList = await _mediator.Send(query, cancellationToken);
                targetLoanIds = filteredList.Items.Select(x => x.Id).ToList();
            }
            else
            {
                targetLoanIds = dto.SelectedIds;
            }

            if (targetLoanIds == null || !targetLoanIds.Any())
                return false;

            var cuser = _currentUserService.GetCurrentUser();

            const int batchSize = 500;
            for (int i = 0; i < targetLoanIds.Count; i += batchSize)
            {
                var currentBatchIds = targetLoanIds.Skip(i).Take(batchSize).ToList();

                var actionEntities = currentBatchIds.Select(loanId => new ActionReferenceEntity
                {
                    Id = Guid.NewGuid(),
                    DataEntryId = loanId,
                    RefType = (Enums.RefTypeEnum) dto.ActionType,
                    ActRefType = Enums.ActRefTypeEnum.Action ,
                    Description = dto.Description,
                    Created = DateTime.Now,
                    FromUserId = cuser.BussinessUserId,
                    ToUserId = cuser.BussinessUserId,
                    IsVisibleForSenator = true,
                }).ToList();
                try
                {
                    await _context.ActionReferences.AddRangeAsync(actionEntities, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {

                    throw;
                }
               
            }

            return true;
        }
    }
}
