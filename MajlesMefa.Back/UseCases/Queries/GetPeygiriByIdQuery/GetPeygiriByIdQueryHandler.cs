using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetPeygiriByIdQuery
{
    public class GetPeygiriByIdQueryHandler : IRequestHandler<GetPeygiriByIdQuery, PeygiryDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPeygiriByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PeygiryDto> Handle(GetPeygiriByIdQuery request, CancellationToken cancellationToken)
        {
            var rslt = await _unitOfWork.PeygiriRepository
                .NoTracking
                .Where(x => x.Id == request.Id)
                .Select(x => new PeygiryDto()
                {
                    Id = x.Id,
                    PeygiriDate = x.PeygiriDate,
                    PeygiriDescription = x.Description,
                    PeygiriKonande = x.PeygiriKonandeId,
                    PeygiriNumber = x.PeygiriNumber,
                    PeygiriKonandeTitle = x.PeygiriKonandeInfo.Name,
                })
                .SingleOrDefaultAsync(cancellationToken);
            return rslt;
        }
    }
}
