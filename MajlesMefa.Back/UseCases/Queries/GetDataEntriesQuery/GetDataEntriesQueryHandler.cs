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
using MajlesMefa.Back.Utilities.Convertor;

namespace MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery
{
    //clean using strategy pattern
    public class GetDataEntriesQueryHandler : IRequestHandler<GetDataEntriesQuery, TableModel<DataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetDataEntriesQueryHandler(RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<DataEntryDto>> Handle(GetDataEntriesQuery request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();

            var query = _context.DataEntries
                .AsNoTracking()
                .Include(x => x.ActionReferences).ThenInclude(x => x.FromUser)
                .Include(x => x.ActionReferences).ThenInclude(x => x.ToUser)
                .Where(x => x.DataEntryType == request.DataEntryType);

            #region permission
            if (cuser.Roles.Any(u => u == RoleTypeEnum.MinistryMember))
            {
                if (request.DataEntryType != DataEntryTypeEnum.DastoorJalasatComission)
                    query = query.Where(q => (q.ActionReferences.Any(ar =>
                    ar.FromUserId == cuser.BussinessUserId ||
                    ar.ToUserId == cuser.BussinessUserId)) ||
                    q.ActionReferences.FirstOrDefault().FromUser.OrganizationId != null);
            }
            #endregion
            if (request.DataEntryId.HasValue)
            {
                query = query.Where(x => x.Id == request.DataEntryId);
            }
            if (request.SenatorIdId.HasValue && !Guid.Empty.Equals(request.SenatorIdId))
            {
                if (request.DataEntryType == DataEntryTypeEnum.TahghighTafahos)
                {
                    var result = query.Include(x => x.TahghighTafahos.TahghighTafahosSenators);
                    query = result.Where(x => (x.SenatorId == request.SenatorIdId) || (x.TahghighTafahos.TahghighTafahosSenators).Where(y => y.SenatorId == request.SenatorIdId).ToList().Count != 0).AsQueryable();
                }
                else
                {
                    query = query.Where(x => x.SenatorId == request.SenatorIdId);
                }
            }

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
            }
            if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            {
                query = query.Where(q => (q.ActionReferences.Any(ar =>
                    ar.ToUserId == cuser.BussinessUserId)));
            }
            //if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            //{
            //    query.Where(q => (q.ActionReferences.Any(ar => ar.FromUserId == cuser.BussinessUserId || ar.ToUserId == cuser.BussinessUserId)) );
            //}

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
            if (request.CurrentUserId.HasValue)
            {
                query = query.Where(x => x.ActionReferences
                            .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                         .OrderByDescending(a => a.Created)
                         .Select(a => a.ToUserId)
                         .FirstOrDefault() == request.CurrentUserId);  
                query = query.Where(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Inprogress);
            }
            return request.DataEntryType switch
            {
                DataEntryTypeEnum.Mokatebe => await ApplyMokatebeFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Tarh => await ApplyTarhFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Layehe => await ApplyLayeheFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Notgh => await ApplyNotghFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Soval => await ApplySovalFilterAsync(query, request.Filter),
                DataEntryTypeEnum.TazakorShafahi => await ApplyTazakorShafahiFilterAsync(query, request.Filter),
                DataEntryTypeEnum.TazakorKatbi => await ApplyTazakorKatbiFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Tazakor => await ApplyTazakorFilterAsync(query, request.Filter),
                DataEntryTypeEnum.EzhaaratResaneee => await ApplyEzhaaratResaneeeFilterAsync(query, request.Filter),
                DataEntryTypeEnum.DastoorJalasatComission => await ApplyDastoorJalasatComissionFilterAsync(query, request.Filter),
                DataEntryTypeEnum.TahghighTafahos => await ApplyTahghighTafahosFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Molaghat => await ApplyMolaghatFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Khadamat => await ApplyKhadamatFilterAsync(query, request.Filter),
                DataEntryTypeEnum.Loan => await ApplyLoanFilterAsync(query, request.Filter),
                _ => throw new NotImplementedException(),
            };
        }


        private async Task<TableModel<DataEntryDto>> ApplyLoanFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {


            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                PersianCreatedDate = x.Created.ToPersianDate("yyyy/MM/dd"),
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new LoanDtailDto()
                {
                    FullName = x.Loan.FullName,
                    NationalNo = x.Loan.NationalNo,
                    MobileNo = x.Loan.MobileNo,
                    Amount = x.Loan.Amount.ShowCurrencyFormat(),
                    LoanType = x.Loan.LoanType,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto(),
                    PasokhNo = x.Loan.PasokhNo,
                    PasokhState = x.Loan.VaziatPasokh,

                }
            }).ToTableResultAsync(filter);

            return result;

        }

        private async Task<TableModel<DataEntryDto>> ApplyKhadamatFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {

            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new KhadamatDto()
                {
                    TarikhKhedmat = x.Khadamat.TarikhKhedmat,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto()
                }
            }).ToTableResultAsync(filter);

            foreach (var item in result.Items)
            {
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
            }
            return result;

        }



        #region DataEntryTypes
        private async Task<TableModel<DataEntryDto>> ApplyNotghFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new NotghDto()
                {
                    AnswerFromProUnitDate = x.Notgh.AnswerFromProUnitDate,
                    AnswerFromProUnitNo = x.Notgh.AnswerFromProUnitNo,
                    JalaseAlaniDate = x.Notgh.JalaseAlaniDate,
                    GardeshErjaat = x.Notgh.GardeshErjaat,
                    Chekide = x.Notgh.Chekide,
                    PasokhNo = x.Notgh.PasokhNo,
                    PasokhState = x.Notgh.PasokhState,
                    PasokhDate = x.Notgh.PasokhDate,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto()
                }

            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }

        //public static string getCommissionTitleById(string id)
        //{
        //    var query = _context.DastoorJalasatComissions.Where(x => x.DataEntryId.ToString() == id).FirstOrDefault();

        //    return query.DataEntry.Title;
        //}

        private async Task<TableModel<DataEntryDto>> ApplyLayeheFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                MyData = new LayeheDto()
                {
                    Type = x.Layehe.Type,
                    ShomareSabt = x.Layehe.ShomareSabt,
                    BarresiKoliatDarSahnDate = x.Layehe.BaresiKoliatDarSahnDate,
                    ElamVosoolDate = x.Layehe.ElamVosoolDate,
                    EblaghDate = x.Layehe.EblaghDate,
                    MajorCommissions = x.Layehe.MajorCommissions != null ? x.Layehe.MajorCommissions.Split(",", StringSplitOptions.None).ToList() : null,
                    MinorCommissions = x.Layehe.MinorCommissions != null ? x.Layehe.MinorCommissions.Split(",", StringSplitOptions.None).ToList() : null,
                    NatijeBarresiCommission = x.Layehe.NatijeBarresiCommission,
                    NatijeBarresiSahn = x.Layehe.NatijeBarresiSahn,
                    NatijeBarresiShora = x.Layehe.NatijeBarresiShora,
                    VazeyatBarresi = x.Layehe.VazeyatBarresi,
                }
            }).ToTableResultAsync(filter);

            foreach (var item in result.Items)
            {
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
            }
            return result;
        }

        private async Task<TableModel<DataEntryDto>> ApplyMokatebeFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
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

            query = query.OrderByDescending(x => x.Created);
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
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
                    Amount = x.Mokatebe.Amount,
                    MokatebeKonande = x.Mokatebe.MokatebeKonande,
                    MokatebeType = x.Mokatebe.MokatebeType,
                    Contact = x.Mokatebe.Contact,
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
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }
        private string GetMoavenatTitle(string id)
        {
            return _context.Categories.Where(x => x.Id == new Guid(id)).Select(x => x.Name).FirstOrDefault();
        }

        private async Task<TableModel<DataEntryDto>> ApplySovalFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var currentUserOrgId = _currentUserService.GetCurrentUser()?.Organization?.Id;
            if (currentUserOrgId.HasValue && _currentUserService.GetCurrentUser().Roles[0] == RoleTypeEnum.Organization)
            {
                query = query.Where(x => x.ActionReferences.Any(a => a.FromUser.OrganizationId == currentUserOrgId || a.ToUser.OrganizationId == currentUserOrgId));
            }
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                CreatorUserName = x.Creator.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value
                } : null,
                MyData = new SovalDto()
                {
                    Commission = x.Soval.Commission,
                    DabirkhaneMakaziNo = x.Soval.DabirkhaneMakaziNo,
                    KarbargDate = x.Soval.KarbargDate,
                    QuestionStatus = x.Soval.QuestionStatus,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto(),
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }
        private async Task<TableModel<DataEntryDto>> ApplyTarhFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new TarhDto()
                {
                    Kollyat = x.Tarh.Kollyat,
                    MavadTarh = x.Tarh.MavadTarh,
                    NatijeBarresi = x.Tarh.NatijeBarresi,
                    NatijeBarresiShoraNegahban = x.Tarh.NatijeBarresiShoraNegahban,
                    Shenase = x.Tarh.Shenase,
                    VazeyatBarresi = x.Tarh.VazeyatBarresi,
                    NatijeBarresiShoraNegahbanDescription = x.Tarh.NatijeBarresiShoraNegahbanDescription,
                    ErsalBeVazir = x.Tarh.ErsalBeVazir,
                    SavabeghEblagh = x.Tarh.SavabeghEblagh,
                    GhozarshNahayi = x.Tarh.GhozarshNahayi,
                    Havashi = x.Tarh.Havashi,
                    HamahangiBaraSherkat = x.Tarh.HamahangiBaraSherkat,
                    Gardeshkar = x.Tarh.Gardeshkar,
                    GozareshMozakerat = x.Tarh.GhozareshMozakerat,
                    NamayandeghanBaraSherkat = x.Tarh.NamayandeghanBaraSherkat,
                    NamayandeghanEmzaKonande = x.Tarh.NamayandeghanEmzaKonande,
                    NazarNamayande = x.Tarh.NazarNamayande,
                    RelatedComission = x.Tarh.RelatedComission

                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
            }
            return result;
        }
        private async Task<TableModel<DataEntryDto>> ApplyTazakorShafahiFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new TazakorShafahiDto()
                {
                    GheraatSahnDate = x.TazakorShafahi.GheraatSahnDate,
                    PasokhNo = x.TazakorShafahi.PasokhNo,
                    PasokhState = x.TazakorShafahi.PasokhState,
                    PasokhDate = x.TazakorShafahi.PasokhDate,
                    PishnevisDate = x.TazakorShafahi.PishnevisDate,
                    ShomareName = x.TazakorShafahi.ShomareName,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto()
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }
        private async Task<TableModel<DataEntryDto>> ApplyTazakorKatbiFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new TazakorKatbiDto()
                {
                    GheraatSahnDate = x.TazakorKatbi.GheraatSahnDate,
                    PishnevisDate = x.TazakorKatbi.PishnevisDate,
                    ShomareName = x.TazakorKatbi.ShomareName,
                    PasokhNo = x.TazakorKatbi.PasokhNo,
                    PasokhState = x.TazakorKatbi.PasokhState,
                    PasokhDate = x.TazakorKatbi.PasokhDate,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto()
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }

        private async Task<TableModel<DataEntryDto>> ApplyTazakorFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new TazakorDto()
                {
                    GheraatSahnDate = x.Tazakor.GheraatSahnDate,
                    PishnevisDate = x.Tazakor.PishnevisDate,
                    ShomareName = x.Tazakor.ShomareName,
                    PasokhNo = x.Tazakor.PasokhNo,
                    PasokhState = x.Tazakor.PasokhState,
                    PasokhDate = x.Tazakor.PasokhDate,
                    TazakorType = x.Tazakor.TazakorType,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto(),
                    NameVaseleDabirkhaneNo = x.Tazakor.NameVaseleDabirkhaneNo,
                    NameVaseleDate = x.Tazakor.NameVaseleDate,
                    NameVaseleNo = x.Tazakor.NameVaseleNo,
                    VaseleAz = x.Tazakor.VaseleAz
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }
        private async Task<TableModel<DataEntryDto>> ApplyEzhaaratResaneeeFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
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

            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new EzhaaratResaneeeDetailDto()
                {
                    Date = x.EzhaaratResaneee.Date,
                    Manba = x.EzhaaratResaneee.Manba,
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
            }
            return result;
        }

        private async Task<TableModel<DataEntryDto>> ApplyDastoorJalasatComissionFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            return await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value
                } : null,
                MyData = new DastoorJalasatComissionDto()
                {
                    DateAndDay = x.DastoorJalasatComission.DateAndDay,
                }
            }).ToTableResultAsync(filter);
        }

        private async Task<TableModel<DataEntryDto>> ApplyTahghighTafahosFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
        {
            var res = query;
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                CommissionTitle = x.TahghighTafahos.DastoorJalasatComission.DataEntry.Title,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value
                } : null,
                MyData = new TahghighTafahosDto()
                {
                    Commission = x.TahghighTafahos.Commission,
                    Tarikh = x.TahghighTafahos.Date,
                    Mokhatab = x.TahghighTafahos.Mokhatab,
                    ShomareDaryaft = x.TahghighTafahos.ShomareDaryaft,
                    ShomareName = x.TahghighTafahos.ShomareName,
                    TahghighTafahosVazyat = x.TahghighTafahos.TahghighTafahosVazyat,
                    //Senators= x.TahghighTafahos.TahghighTafahosSenators,
                    AccessActionRefrence = new GetAccessActionRefrenceResultDto()
                }
            }).ToTableResultAsync(filter);
            foreach (var item in result.Items)
            {
                var id = (Guid)(item.GetType().GetProperty("Id").GetValue(item));
                item.CategoryParentName = item.Moavenats != null ? string.Join(" - ", item.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : item.CategoryParentName;
                item.MyData.GetType().GetProperty("AccessActionRefrence").SetValue(item.MyData, CheckAccess(id).Result);
            }
            return result;
        }

        private async Task<TableModel<DataEntryDto>> ApplyMolaghatFilterAsync(IQueryable<DataEntryEntity> query, TableRequestModel filter)
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

            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null ? x.Category.Parent.Name : x.Category.Name,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                CommissionTitle = x.Senator.SenatorProfile.Commission.DataEntryId.ToString(),
                //CommissionTitle = await GetCommissionById(x.Senator.SenatorProfile.Commission.DataEntryId.ToString()) ,
                SenatorCity = x.Senator.City.Name,
                SenatorName = x.Senator.SenatorProfile.Name,
                SenatorHozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeEntekhabi.GetPersianName(),
                    City = x.Senator.City.Name,
                } : null,
                MyData = new MolaghatDto()
                {
                    Count = x.Molaghat.Count,
                    PasokhNo = x.Molaghat.PasokhNo,
                    PasokhState = x.Molaghat.PasokhState,
                    Mahal = x.Molaghat.Mahal,
                    MolaghatType = x.Molaghat.MolaghatType,
                    Tarikh = x.Molaghat.Date,
                    IsMolaghatBaVazir = x.Molaghat.IsMolaghatBaVazir
                }
            }).ToTableResultAsync(filter);
            var myData = result.Items;
            var total = result.Count;
            for (var i = 0; i < myData.Count; i++)
            {
                myData[i].CommissionTitle = await GetCommissionById(myData[i].CommissionTitle);
            }
            //return await myData.ToTableResultAsync(filter); 
            var finalData = myData.ToTableResult();
            finalData.Count = total;
            return finalData;
        }

        private async Task<string> GetCommissionById(string id)
        {
            return _context.DataEntries.Where(x => x.Id == new Guid(id)).FirstOrDefault().Title;
        }
        private async Task<GetAccessActionRefrenceResultDto> CheckAccess(Guid dataEntryId)
        {
            var cuser = _currentUserService.GetCurrentUser();
            var result = new GetAccessActionRefrenceResultDto();
            if (cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin || x == RoleTypeEnum.Admin || x == RoleTypeEnum.MinistryMember))
            {
                result.AccessRefrence = true;
                result.AccessAction = true;
            }
            else
            {
                var hasAccessList = await _context.ActionReferences
                   .Where(x => x.DataEntryId == dataEntryId).OrderByDescending(o => o.Created)
                   .ToListAsync();
                var hasAccessAction = hasAccessList?.Where(x => x.ActRefType != ActRefTypeEnum.Refer)
                   .FirstOrDefault();
                var hasAccessRefer = hasAccessList?.Where(x => x.ActRefType == ActRefTypeEnum.Refer)
                   .FirstOrDefault();
                if (hasAccessRefer != null && hasAccessRefer?.ToUserId == cuser.BussinessUserId)
                {
                    result.AccessRefrence = true;
                    result.AccessAction = true;
                }

            }
            return result;
        }

        #endregion
    }

}
