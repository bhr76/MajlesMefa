using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Repositories;

namespace MajlesMefa.Back.UseCases.Queries.GetOrganizationsQuery
{
    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrganizationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrganizationDto> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.OrganizationRepositroy
                .NoTracking
                .Where(x => x.Id == request.Id);

                var rslt = await query.Select(s => new OrganizationDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .SingleOrDefaultAsync(cancellationToken);
            return rslt;
        }
    }
}
