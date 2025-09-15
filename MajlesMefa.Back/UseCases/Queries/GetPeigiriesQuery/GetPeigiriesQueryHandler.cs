using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetPeigiriesQuery
{
    public class GetPeigiriesQueryHandler : IRequestHandler<GetPeigiriesQuery, TableModel<PeygiryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPeigiriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<PeygiryDto>> Handle(GetPeigiriesQuery request, CancellationToken cancellationToken)
        {
            var rslt = await _unitOfWork.PeygiriRepository
                .NoTracking
                .Where(x => x.DataEntryId == request.DataEntryId)
                .Select(x => new PeygiryDto()
                {
                    Id = x.Id,
                    DataEntryId= x.DataEntryId,
                    PeygiriDate = x.PeygiriDate,
                    PeygiriDescription = x.Description,
                    PeygiriKonande = x.PeygiriKonandeId,
                    PeygiriNumber = x.PeygiriNumber,
                    PeygiriKonandeTitle = x.PeygiriKonandeInfo.Name,
                })
                .ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
