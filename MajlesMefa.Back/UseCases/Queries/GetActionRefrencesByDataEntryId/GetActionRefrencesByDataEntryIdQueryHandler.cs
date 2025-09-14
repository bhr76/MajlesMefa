using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MajlesMefa.Back.UseCases.Queries.GetActionRefrencesByDataEntryId
{
    public class GetActionRefrencesByDataEntryIdQueryHandler : IRequestHandler<GetActionRefrencesByDataEntryIdQuery, TableModel<ActionRefrenceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetActionRefrencesByDataEntryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<ActionRefrenceDto>> Handle(GetActionRefrencesByDataEntryIdQuery request, CancellationToken cancellationToken)
        {
            var rslt = _unitOfWork.ActionReferenceRepository
                .NoTracking
                .Where(o => o.DataEntryId == request.DataEntryId
                && o.ActRefType != ActRefTypeEnum.Create
                && o.ActRefType != ActRefTypeEnum.Edit);
            if (request.ActRefType == ActRefTypeEnum.Refer)
            {
                rslt = rslt.Where(x => x.ActRefType == ActRefTypeEnum.Refer);
            }
            else
            {
                rslt = rslt.Where(x => x.ActRefType != ActRefTypeEnum.Refer);
            }
            var response = await rslt.Select(s => new ActionRefrenceDto()
            {
                Action = s.ActRefType,
                RefType = s.RefType,
                ActionDesc=s.ActRefType.GetDisplayName(),
                RefTypeDesc=s.RefType != 0 ? s.RefType.GetDisplayName() : "",
                ActionerCityName = s.FromUser.City.Name,
                ActionerName = s.FromUser.Name,
                ActionerOrgName = s.FromUser.Organization.Name,
                ActionerParentCityName = s.FromUser.City.Parent.Name,
                ActionerParentOrgName = s.FromUser.Organization.Parent.Name,
                Created = s.Created.ToPersianDateTime("yyyy/MM/dd"),
                Description = s.Description,
                Id = s.Id,
                RefCityName = s.ActRefType == ActRefTypeEnum.Refer ? s.ToUser.City.Name : null,
                RefOrgName = s.ActRefType == ActRefTypeEnum.Refer ? s.ToUser.Organization.Name : null,
                RefParentCityName = s.ActRefType == ActRefTypeEnum.Refer ? s.ToUser.City.Parent.Name : null,
                RefParentOrgName = s.ActRefType == ActRefTypeEnum.Refer ? s.ToUser.Organization.Parent.Name : null,
                RefUserName = s.ActRefType == ActRefTypeEnum.Refer ? s.ToUser.Name : null,
            }).ToTableResultAsync(request.Filter);
            return response;
        }
    }
}
