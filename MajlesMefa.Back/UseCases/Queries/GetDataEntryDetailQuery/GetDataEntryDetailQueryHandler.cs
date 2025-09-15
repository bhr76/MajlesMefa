using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common.Grid;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery
{
    public class GetDataEntryDetailQueryHandler : IRequestHandler<GetDataEntryDetailQuery, DataEntryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RefahMajlesDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetDataEntryDetailQueryHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            RefahMajlesDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<DataEntryDto> Handle(GetDataEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();

            var query = _unitOfWork.DataEntryRepository
                .NoTracking
                .Where(x => x.Id == request.DataEntryId)
                .Where(x => x.DataEntryType == request.DataEntryType);

            #region permission
            if (!cuser.Roles.Any(u => u == Enums.RoleTypeEnum.MinistryMember || u == Enums.RoleTypeEnum.MinistryAdmin))
            {
                //query = query.Where(q => q.ActionReferences.Any(ar => ar.FromUserId == cuser.UserId || ar.ToUserId == cuser.UserId));
            }
            #endregion

            return request.DataEntryType switch
            {
                DataEntryTypeEnum.Mokatebe => await ApplyMokatebeFilterAsync(query),
                DataEntryTypeEnum.Tarh => await ApplyTarhFilterAsync(query),
                DataEntryTypeEnum.Layehe => await ApplyLayeheFilterAsync(query),
                DataEntryTypeEnum.Notgh => await ApplyNotghFilterAsync(query),
                DataEntryTypeEnum.Soval => await ApplySovalFilterAsync(query),
                DataEntryTypeEnum.TazakorKatbi => await ApplyTazakorKatbiFilterAsync(query),
                DataEntryTypeEnum.TazakorShafahi => await ApplyTazakorShafahiFilterAsync(query),
                DataEntryTypeEnum.Tazakor => await ApplyTazakorFilterAsync(query),
                DataEntryTypeEnum.EzhaaratResaneee => await ApplyEzhaaratResaneeeFilterAsync(query),
                DataEntryTypeEnum.DastoorJalasatComission => await ApplyDastoorJalasatComissionFilterAsync(query, request.DataEntryId),
                DataEntryTypeEnum.TahghighTafahos => await ApplyTahghighTafahosFilterAsync(query, request.DataEntryId),
                DataEntryTypeEnum.Molaghat => await ApplyMolaghatFilterAsync(query),
                DataEntryTypeEnum.Khadamat => await ApplyKhadamatFilterAsync(query),
                _ => throw new NotImplementedException(),
            };
        }


        #region DataEntryTypes

        private async Task<DataEntryDto> ApplyKhadamatFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                MyData = new KhadamatDetailDto()
                {
                    
                    TarikhKhedmat = x.Khadamat.TarikhKhedmat,
                    Khedmat = x.Khadamat.Khedmat,
                    Id = x.Khadamat.DataEntryId
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }
        private async Task<DataEntryDto> ApplyNotghFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                MyData = new NotghDetailDto()
                {
                    AnswerFromProUnitDate = x.Notgh.AnswerFromProUnitDate,
                    PasokhNo = x.Notgh.PasokhNo,
                    PasokhState = x.Notgh.PasokhState,
                    PasokhDate = x.Notgh.PasokhDate,
                    AnswerFromProUnitNo = x.Notgh.AnswerFromProUnitNo,
                    Chekide = x.Notgh.Chekide,
                    GardeshErjaat = x.Notgh.GardeshErjaat,
                    JalaseAlaniDate = x.Notgh.JalaseAlaniDate,
                    Peygiries = x.Peygiries.OrderByDescending(x => x.Created)
                    .Select(s => new PeygiryDto()
                    {
                        Id = s.Id,
                        PeygiriDate = s.PeygiriDate,
                        PeygiriDescription = s.Description,
                        PeygiriKonande = s.PeygiriKonandeId,
                        PeygiriNumber = s.PeygiriNumber
                    })
                    .ToList()
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplyTarhFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                MyData = new TarhDetailDto()
                {
                    Gardeshkar = x.Tarh.Gardeshkar,
                    GhozareshMozakerat = x.Tarh.GhozareshMozakerat,
                    GhozarshNahayi = x.Tarh.GhozarshNahayi,
                    HamahangiBaraSherkat = x.Tarh.HamahangiBaraSherkat,
                    Havashi = x.Tarh.Havashi,
                    ErsalBeVazir = x.Tarh.ErsalBeVazir,
                    Keywords = x.Keywords.Select(y => new KeywordDto()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        RepeatCount = y.RepeatCount,
                    }).ToList(),
                    Kollyat = x.Tarh.Kollyat,
                    MavadTarh = x.Tarh.MavadTarh,
                    NamayandeghanBaraSherkat = x.Tarh.NamayandeghanBaraSherkat,
                    NamayandeghanEmzaKonande = x.Tarh.NamayandeghanEmzaKonande,
                    NatijeBarresi = x.Tarh.NatijeBarresi,
                    NatijeBarresiShoraNegahban = x.Tarh.NatijeBarresiShoraNegahban,
                    NatijeBarresiShoraNegahbanDescription = x.Tarh.NatijeBarresiShoraNegahbanDescription,
                    NazarNamayande = x.Tarh.NazarNamayande,
                    RelatedComission = x.Tarh.RelatedComission,
                    SavabeghEblagh = x.Tarh.SavabeghEblagh,
                    Shenase = x.Tarh.Shenase,
                    VazeyatBarresi = x.Tarh.VazeyatBarresi
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplyLayeheFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                MyData = new LayeheDetailDto()
                {
                    Type = x.Layehe.Type,
                    ShomareSabt = x.Layehe.ShomareSabt,
                    BaresiKoliatDarSahnDate = x.Layehe.BaresiKoliatDarSahnDate,
                    ElamVosoolDate = x.Layehe.ElamVosoolDate,
                    EblaghDate = x.Layehe.EblaghDate,
                    MajorCommissionsStr = x.Layehe.MajorCommissions != null ? x.Layehe.MajorCommissions.Split(",", StringSplitOptions.None).ToList() : null,
                    MinorCommissionsStr = x.Layehe.MinorCommissions != null ? x.Layehe.MinorCommissions.Split(",", StringSplitOptions.None).ToList() : null,
                    NatijeBarresiCommission = x.Layehe.NatijeBarresiCommission,
                    NatijeBarresiSahn = x.Layehe.NatijeBarresiSahn,
                    NatijeBarresiShora = x.Layehe.NatijeBarresiShora,
                    VazeyatBarresi = x.Layehe.VazeyatBarresi,
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplySovalFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                    City = x.Senator.City.Name,
                } : null,
                MyData = new SovalDetailDto()
                {
                    BarresiDarCommission = x.Soval.BarresiDarCommission,
                    BarresiDarSahn = x.Soval.BarresiDarSahn,
                    Commission = x.Soval.Commission,
                    DabirkhaneMakaziNo = x.Soval.DabirkhaneMakaziNo,
                    JalasatDakheli = x.Soval.JalasatDakheli,
                    KarbargDate = x.Soval.KarbargDate,
                    QuestionStatus = x.Soval.QuestionStatus,
                    Peygiries = x.Peygiries.OrderByDescending(x => x.Created)
                     .Select(s => new PeygiryDto()
                     {
                         Id = s.Id,
                         PeygiriDate = s.PeygiriDate,
                         PeygiriDescription = s.Description,
                         PeygiriKonande = s.PeygiriKonandeId,
                         PeygiriNumber = s.PeygiriNumber
                     })
                     .ToList()
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private string GetMoavenatTitle(string id)
        {
            return _dbContext.Categories.Where(x => x.Id == new Guid(id)).Select(x => x.Name).FirstOrDefault();
        }

        private async Task<DataEntryDto> ApplyTazakorKatbiFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                Id = x.Id,
                MyData = new TazakorKatbiDetailDto()
                {
                    GheraatSahnDate = x.TazakorKatbi.GheraatSahnDate,
                    PishnevisDate = x.TazakorKatbi.PishnevisDate,
                    ShomareName = x.TazakorKatbi.ShomareName,
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }
        
        private async Task<DataEntryDto> ApplyTazakorShafahiFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                MyData = new TazakorShafahiDetailDto()
                {
                    GheraatSahnDate = x.TazakorShafahi.GheraatSahnDate,
                    PishnevisDate = x.TazakorShafahi.PishnevisDate,
                    ShomareName = x.TazakorShafahi.ShomareName,
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplyTazakorFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                MyData = new TazakorDetailDto()
                {
                    GheraatSahnDate = x.Tazakor.GheraatSahnDate,
                    PishnevisDate = x.Tazakor.PishnevisDate,
                    ShomareName = x.Tazakor.ShomareName,
                    TazakorType = x.Tazakor.TazakorType,
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }
        private async Task<DataEntryDto> ApplyEzhaaratResaneeeFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                MyData = new EzhaaratResaneeeDetailDto()
                {
                    Date = x.EzhaaratResaneee.Date,
                    Manba = x.EzhaaratResaneee.Manba,
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplyDastoorJalasatComissionFilterAsync(IQueryable<DataEntryEntity> query, Guid dataEntryId)
        {

            var relatedSoal = _dbContext.Sovals.Where(x => x.Commission == dataEntryId).Any();
            var relatedTarh = _dbContext.Tarhs.Where(x => x.RelatedComission == dataEntryId).Any();
            var relatedTahghigh = _dbContext.TahghighTafahoses.Where(x => x.Commission == dataEntryId).Any();




            var result = await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
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
                MyData = new DastoorJalasatComissionDetailDto()
                {
                    DateAndDay = x.DastoorJalasatComission.DateAndDay,
                    Ghozareshat = x.DastoorJalasatComission.Ghozareshat,
                    HasRelation = relatedSoal || relatedTahghigh || relatedTarh,
                }
            }).SingleOrDefaultAsync();
            return result;
        }

        private async Task<DataEntryDto> ApplyTahghighTafahosFilterAsync(IQueryable<DataEntryEntity> query, Guid dataEntryId)
        {
            var senators = await _unitOfWork.TahghighTafahosSenatorReposity.NoTracking
                .Include(x => x.Senator)
                .Where(x => x.TahghighTafahosId == dataEntryId)
                .Select(x => new
                {
                    Id = x.SenatorId,
                    Name = x.Senator.Name
                }).ToListAsync();
            var senatorIds = senators.Select(x => x.Id).ToList();
            var senatorNames = senators.Select(x => x.Name);
            var result = await query
                .Select(x => new DataEntryDto()
                {
                    CreatorUserName = x.Creator.Name,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    CategoryParentId = x.Category.ParentId,
                    CategoryParentName = x.Category.Parent.Name,
                    Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
                    DataEntryType = x.DataEntryType,
                    Description = x.Description,
                    Title = x.Title,
                    Id = x.Id,
                    Senator = x.SenatorId.HasValue ? new SenatorDto()
                    {
                        Name = x.Senator.SenatorProfile.Name,
                        UserId = x.SenatorId.Value,
                        HozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                        City = x.Senator.City.Name,
                    } : null,
                    MyData = new TahghighTafahosDetailDto()
                    {
                        Commission = x.TahghighTafahos.Commission,
                        Date = x.TahghighTafahos.Date,
                        Mokhatab = x.TahghighTafahos.Mokhatab,
                        ShomareDaryaft = x.TahghighTafahos.ShomareDaryaft,
                        ShomareName = x.TahghighTafahos.ShomareName,
                        TahghighTafahosVazyat = x.TahghighTafahos.TahghighTafahosVazyat,
                        TahghighTafahosSenatorsId = senatorIds,
                        TahghighTafahosSenatorsName = String.Join(" - ", senatorNames),
                    }
                }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplyMolaghatFilterAsync(IQueryable<DataEntryEntity> query)
        {
            var result = await query.Select(x => new DataEntryDto()
            {
                CreatorUserName = x.Creator.Name,
                DataEntryType = x.DataEntryType,
                Description = x.Description,
                Title = x.Title,
                CommissionTitle = x.Senator.SenatorProfile.Commission.DataEntryId.ToString(),
                Id = x.Id,
                Senator = x.SenatorId.HasValue ? new SenatorDto()
                {
                    Name = x.Senator.SenatorProfile.Name,
                    UserId = x.SenatorId.Value,
                    HozeEntekhabi = x.Senator.SenatorProfile.HozeCity.Name,
                    City = x.Senator.City.Name,
                } : null,
                MyData = new MolaghatDtailDto()
                {

                    Count = x.Molaghat.Count,
                    Mahal = x.Molaghat.Mahal,
                    MolaghatType = x.Molaghat.MolaghatType,
                    Date = x.Molaghat.Date,
                    PasokhNo = x.Molaghat.PasokhNo,
                    PasokhState = x.Molaghat.PasokhState,
                    PasokhDate = x.Molaghat.PasokhDate,
                    Id = x.Molaghat.DataEntryId,
                    IsMolaghatBaVazir = x.Molaghat.IsMolaghatBaVazir
                }
            }).SingleOrDefaultAsync();
            result.CategoryParentName = result.Moavenats != null ? string.Join(" - ", result.Moavenats?.Select(x => GetMoavenatTitle(x)).ToList()) : result.CategoryParentName;
            return result;
        }

        private async Task<DataEntryDto> ApplyMokatebeFilterAsync(IQueryable<DataEntryEntity> query)
        {
            return await query.Select(x => new DataEntryDto()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                CategoryParentId = x.Category.ParentId,
                CategoryParentName = x.Category.Parent.Name,
                Moavenats = x.Moavenats != null ? x.Moavenats.Split(",", StringSplitOptions.None).ToList() : null,
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
                    Peygiries = x.Peygiries.OrderByDescending(x => x.Created)
                    .Select(s => new PeygiryDto()
                    {
                        Id = s.Id,
                        PeygiriDate = s.PeygiriDate,
                        PeygiriDescription = s.Description,
                        PeygiriKonande = s.PeygiriKonandeId,
                        PeygiriNumber = s.PeygiriNumber
                    })
                    .ToList()
                }
            }).SingleOrDefaultAsync();
        }
        #endregion
    }
}
