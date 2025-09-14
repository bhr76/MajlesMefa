using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Dtos.Common.Grid;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.FilterDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.Pagination;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery
{
    //clean using strategy pattern
    public class GetMokatebeDataQueryHandler : IRequestHandler<GetMokatebeDataQuery, TableModel<MokatebeDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetMokatebeDataQueryHandler(RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<MokatebeDto>> Handle(GetMokatebeDataQuery request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();

            var query = _context.DataEntries
                .AsNoTracking()
                .Include(x => x.Peygiries)
                .Include(x => x.ActionReferences).ThenInclude(x => x.FromUser)
                .Include(x => x.ActionReferences).ThenInclude(x => x.ToUser)
                .Where(x => x.DataEntryType == request.DataEntryType);

            #region permission
            if (cuser.Roles.Any(u => u == RoleTypeEnum.MinistryMember))
            {
                if(request.DataEntryType != DataEntryTypeEnum.DastoorJalasatComission)
                    query = query.Where(q => (q.ActionReferences.Any(ar => ar.FromUserId == cuser.BussinessUserId || ar.ToUserId == cuser.BussinessUserId)) || q.ActionReferences.FirstOrDefault().FromUser.OrganizationId != null);
            }
            #endregion
            if (request.DataEntryId.HasValue)
            {
                query = query.Where(x => x.Id == request.DataEntryId);
            }
            if (request.SenatorIdId.HasValue && !Guid.Empty.Equals(request.SenatorIdId))
            {
                query = query.Where(x => x.SenatorId == request.SenatorIdId);
            }

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
            }

            if (request.IsNeedUserAction)
            {
                query = query.Where(q => q.ActionReferences.OrderByDescending(x => x.Created).First().ToUserId == cuser.UserId
                    && q.ActionReferences.OrderByDescending(x => x.Created).First().FromUserId != cuser.UserId);
            }
            if (request.IsBelongCurrentUser)
            {
                query = query.Where(q => q.ActionReferences.OrderBy(x => x.Created).First().FromUserId == cuser.UserId);
            }
            if (request.RelatedSenatorId.HasValue)
            {
                query = query.Where(q => q.SenatorId == request.RelatedSenatorId);
            }
            if (request.CategoryId.HasValue)
            {
                query = query.Where(q => q.CategoryId == request.CategoryId);
            }
            if (request.Title?.Length > 0)
            {
                query = query.Where(q => q.Title.Contains(request.Title));
            }

            return request.DataEntryType switch
            {
                DataEntryTypeEnum.Mokatebe => await ApplyMokatebeFilterAsync(query, request.Filter),
                _ => throw new NotImplementedException(),
            };
        }


        #region DataEntryTypes

        private async Task<TableModel<MokatebeDto>> ApplyMokatebeFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var currentUserOrgId = _currentUserService.GetCurrentUser()?.Organization?.Id;
            if (currentUserOrgId.HasValue && _currentUserService.GetCurrentUser().Roles[0] == RoleTypeEnum.Organization)
            {
                query = query.Where(x => x.ActionReferences.Any(a => a.FromUser.OrganizationId == currentUserOrgId || a.ToUser.OrganizationId == currentUserOrgId));
            }

            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser.Roles.Any(r => r == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == currentUser.BussinessUserId);
            }
           
            var result= await query.Select(x => new MokatebeDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                ShomareDabirkhaneMarkazi = x.Mokatebe.ShomareDabirkhaneMarkazi,
                ShomareDabirkhane = x.Mokatebe.ShomareDabirkhane,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabiEnum = x.Senator.SenatorProfile.HozeEntekhabi,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new MokatebeRsltDto()
                {
                    Amount =x.Mokatebe.Amount,
                    MokatebeKonande = x.Mokatebe.MokatebeKonande,
                    MokatebeType = x.Mokatebe.MokatebeType,
                    Contact= x.Mokatebe.Contact,
                    VaziatPasokh = x.Mokatebe.VaziatPasokh,
                    ShomareDabirkhane = x.Mokatebe.ShomareDabirkhane,
                    PasokhNo = x.Mokatebe.PasokhNo,
                    PasokhDate = x.Mokatebe.PasokhDate,
                    ShomareDabirkhaneMarkazi = x.Mokatebe.ShomareDabirkhaneMarkazi,
                    TarikhDabirKhane = x.Mokatebe.TarikhDabirKhane,
                    TarikhDabirMarkazi = x.Mokatebe.TarikhDabirKhaneMarkazi,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto(),
                    Peygiries = x.Peygiries.OrderByDescending(x => x.Created)
                    .Select(s => new PeygiryDto()
                    {
                        Id = s.Id,
                        PeygiriDate = s.PeygiriDate,
                        PeygiriDescription = s.Description,
                        PeygiriKonande = s.PeygiriKonandeId,
                        PeygiriNumber = s.PeygiriNumber,
                    })
                    .ToList()
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ?  string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }
        
        private string GetMoavenatTitle (string id)
        {
            return _context.Categories.Where(x => x.Id == new Guid(id)).Select(x => x.Name).FirstOrDefault();
        }
        
        private async Task<GetAccessActionRefrenceResultDto> CheckAccess(Guid dataEntryId)
        {
             var cuser =  _currentUserService.GetCurrentUser();
            var result = new GetAccessActionRefrenceResultDto();
            if (cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin || x == RoleTypeEnum.Admin || x == RoleTypeEnum.Organization || x == RoleTypeEnum.MinistryMember))
            {
                result.AccessRefrence = true;
                result.AccessAction = true;
            }
            else
            {
                var hasAccessList =await  _context.ActionReferences
                   .Where(x => x.DataEntryId == dataEntryId).OrderByDescending(o => o.Created)
                   .ToListAsync();
                var hasAccessAction = hasAccessList?.Where(x=>x.ActRefType != ActRefTypeEnum.Refer)
                   .FirstOrDefault();
                var hasAccessRefer = hasAccessList?.Where(x => x.ActRefType == ActRefTypeEnum.Refer)
                   .FirstOrDefault();
                if (hasAccessRefer != null && hasAccessRefer?.ToUserId== cuser.BussinessUserId) { 
                    result.AccessRefrence = true;
                    result.AccessAction = true;
                }
               
            }
            return result;
        }

        #endregion
    }

}
