using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetKeywordsQuery
{
    public class GetKeywordsQueryHandler : IRequestHandler<GetKeywordsQuery, GetKeywordsQueryResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetKeywordsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetKeywordsQueryResult> Handle(GetKeywordsQuery request, CancellationToken cancellationToken)
        {
            var totalSum = await _unitOfWork.KeywordRepository
                .NoTracking
                .Where(x => x.DataEntryId == request.DataEntryId)
                .GroupBy(x => x.DataEntryId)
                .Select(x => x.Sum(y => y.RepeatCount))
                .SingleOrDefaultAsync(cancellationToken);
            var rslt = new GetKeywordsQueryResult()
            {
                Sum = totalSum
            };
            rslt.Keywords = await _unitOfWork.KeywordRepository
                .NoTracking
                .Where(x => x.DataEntryId == request.DataEntryId)
                .Select(x => new KeywordDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    RepeatCount = x.RepeatCount
                }).ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
