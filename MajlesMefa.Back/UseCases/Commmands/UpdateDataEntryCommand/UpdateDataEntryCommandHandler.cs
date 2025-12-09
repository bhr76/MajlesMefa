using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MajlesMefa.Back.Repositories;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand
{
    public class UpdateDataEntryCommandHandler : IRequestHandler<UpdateDataEntryCommand>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UpdateDataEntryCommandHandler(RefahMajlesDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task Handle(UpdateDataEntryCommand request, CancellationToken cancellationToken)
        {
            ISpecification<ActionReferenceEntity> specification = new EditableDataEntrySpecification();
            var cuser = _currentUserService.GetCurrentUser();

            var data = await _context.
               ActionReferences
               .Include(x => x.FromUser)
               .Include(x => x.DataEntry)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Notgh)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.TazakorShafahi)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.TazakorKatbi)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Tazakor)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Tarh)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Mokatebe)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Layehe)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Soval)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.EzhaaratResaneee)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.DastoorJalasatComission)
               .Include(x => x.DataEntry)
               .ThenInclude(x=> x.Loan)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.TahghighTafahos)
               .Include(x => x.DataEntry)
               .ThenInclude(x => x.Molaghat)
               .Include(x => x.DataEntry)
               .Where(x => x.DataEntryId == request.DataEntryId)
               .OrderByDescending(s => s.Created)
               .FirstOrDefaultAsync(cancellationToken);

            var creator = await _context.ActionReferences
               .Where(x => x.DataEntryId == request.DataEntryId)
               .OrderBy(s => s.Created)
               .FirstOrDefaultAsync(cancellationToken);
            //if ((cuser.Roles.Any(x => x.Equals(RoleTypeEnum.Organization)) || cuser.Roles.Any(x => x.Equals(RoleTypeEnum.Senator))) && cuser.BussinessUserId != creator.FromUserId)
            if (cuser.Roles.Any(x => x.Equals(RoleTypeEnum.Senator)) && cuser.BussinessUserId != creator.FromUserId)
            {
                throw new InvalidOperationException("موجودیت انتخابی قابل ویرایش نمی باشد");
            }
            //if (/*!specification.IsSatisfied(data) || */ (cuser.BussinessUserId != data.FromUserId && !(cuser.Roles.Any(x => x.Equals(RoleTypeEnum.Admin))) && !(cuser.Roles.Any(x => x.Equals(RoleTypeEnum.MinistryAdmin))) && !(cuser.Roles.Any(x => x.Equals(RoleTypeEnum.MinistryMember)))))
            //{
            //    throw new InvalidOperationException("موجودیت انتخابی قابل ویرایش نمی باشد");
            //}

            var preType = data.DataEntry.DataEntryType;
            var newType = request.DataEntryType;
            data.DataEntry = _mapper.Map(request, data.DataEntry);
            //if (request.CategoryId.HasValue)
            //{
            //    var category = await _context.Categories
            //        .Where(c => c.Id == request.CategoryId)
            //        .Select(c => c.DataEntryType)
            //        .SingleOrDefaultAsync(cancellationToken);
            //    if(category.HasValue && category != newType)
            //    {
            //        throw new InvalidOperationException("نوع دسته بندی با نوع موجودیت انتخابی تطابق ندارد");
            //    }
            //}
            data.DataEntry.Moavenats = request.Moavenats != null ? string.Join(",", request.Moavenats) : null;
            data.DataEntry.CategoryId = request.CategoryId;

            


            if (preType != newType)
            {
                switch (newType)
                {
                    case DataEntryTypeEnum.Notgh:
                        data.DataEntry.Notgh = _mapper.Map<NotghEntity>(request.DataEntryData as NotghDetailDto);
                        break;
                    case DataEntryTypeEnum.TazakorKatbi:
                        data.DataEntry.TazakorKatbi = _mapper.Map<TazakorKatbiEntity>(request.DataEntryData as TazakorKatbiDetailDto);
                        break;
                    case DataEntryTypeEnum.TazakorShafahi:
                        data.DataEntry.TazakorShafahi = _mapper.Map<TazakorShafahiEntity>(request.DataEntryData as TazakorShafahiDetailDto);
                        break;
                    case DataEntryTypeEnum.Tazakor:
                        data.DataEntry.Tazakor = _mapper.Map<TazakorEntity>(request.DataEntryData as TazakorDetailDto);
                        break;
                    case DataEntryTypeEnum.Mokatebe:
                        data.DataEntry.Mokatebe = _mapper.Map<MokatebeEntity>(request.DataEntryData as MokatebeInputDto);
                        break;
                    case DataEntryTypeEnum.Tarh:
                        data.DataEntry.Tarh = _mapper.Map<TarhEntity>(request.DataEntryData as TarhDetailDto);
                        break;
                    case DataEntryTypeEnum.Layehe:
                        var majors = ((LayeheDetailDto)request.DataEntryData).MajorCommissions;
                        var minors = ((LayeheDetailDto)request.DataEntryData).MinorCommissions;
                        data.DataEntry.Layehe = _mapper.Map<LayeheEntity>(request.DataEntryData as LayeheDetailDto);
                        data.DataEntry.Layehe.MajorCommissions = majors != null ? string.Join(",", majors) : null;
                        data.DataEntry.Layehe.MinorCommissions = minors != null ? string.Join(",", minors) : null;
                        break;
                    case DataEntryTypeEnum.Soval:
                        data.DataEntry.Soval = _mapper.Map<SovalEntity>(request.DataEntryData as SovalDetailDto);
                        break;
                    case DataEntryTypeEnum.EzhaaratResaneee:
                        data.DataEntry.EzhaaratResaneee = _mapper.Map<EzhaaratResaneeeEntity>(request.DataEntryData as EzhaaratResaneeeDetailDto);
                        break;
                    case DataEntryTypeEnum.DastoorJalasatComission:
                        data.DataEntry.DastoorJalasatComission = _mapper.Map<DastoorJalasatComissionEntity>(request.DataEntryData as DastoorJalasatComissionDetailDto);
                        break;
                    case DataEntryTypeEnum.TahghighTafahos:
                        data.DataEntry.TahghighTafahos = _mapper.Map<TahghighTafahosEntity>(request.DataEntryData as TahghighTafahosDetailDto);
                        var senatorIds = ((TahghighTafahosDetailDto)request.DataEntryData).TahghighTafahosSenatorsId;
                        var currentSenators = await _unitOfWork.TahghighTafahosSenatorReposity
                            .NoTracking
                            .Where(r => r.TahghighTafahosId == data.DataEntryId)
                            .Select(x => new Guid(
                                x.SenatorId.ToString())
                            ).ToListAsync(cancellationToken);
                        var mustAdd = senatorIds?.Except(currentSenators);
                        var mustRemove = senatorIds != null ? currentSenators?.Except(senatorIds) : currentSenators;
                        var tahghighTafahos = await _unitOfWork.TahghighTafahosReposity
                            .Tracking
                            .Include(x => x.TahghighTafahosSenators)
                            .Where(x => x.DataEntryId == data.DataEntryId)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(cancellationToken);
                        if (mustRemove?.Count() > 0)
                        {
                            foreach (var item in mustRemove)
                            {
                                await _unitOfWork.TahghighTafahosReposity.RemoveSenatorAsync(data.DataEntryId, item);
                            }
                        }

                        if (mustAdd?.Count() > 0)
                        {
                            foreach (var item in mustAdd)
                            {
                                await _unitOfWork.TahghighTafahosReposity.AddSenatorAsync(data.DataEntry.TahghighTafahos, item);
                            }
                        }
                        break;
                    case DataEntryTypeEnum.Molaghat:
                        data.DataEntry.Molaghat = _mapper.Map<MolaghatEntity>(request.DataEntryData as MolaghatDtailDto);
                        break;
                    default:
                        throw new InvalidOperationException("نوع دیتا تعریف نشده است");
                }
            }
            else
            {
                switch (request.DataEntryType)
                {
                    case DataEntryTypeEnum.Notgh:
                        data.DataEntry.Notgh = _mapper.Map(request.DataEntryData as NotghDetailDto, data.DataEntry.Notgh);
                        break;
                    case DataEntryTypeEnum.TazakorKatbi:
                        data.DataEntry.TazakorKatbi = _mapper.Map(request.DataEntryData as TazakorKatbiDetailDto, data.DataEntry.TazakorKatbi);
                        break;
                    case DataEntryTypeEnum.TazakorShafahi:
                        data.DataEntry.TazakorShafahi = _mapper.Map(request.DataEntryData as TazakorShafahiDetailDto, data.DataEntry.TazakorShafahi);
                        break;
                    case DataEntryTypeEnum.Tazakor:
                        data.DataEntry.Tazakor = _mapper.Map(request.DataEntryData as TazakorDetailDto, data.DataEntry.Tazakor);
                        break;
                    case DataEntryTypeEnum.Mokatebe:
                        data.DataEntry.Mokatebe = _mapper.Map(request.DataEntryData as MokatebeInputDto, data.DataEntry.Mokatebe);
                        data.DataEntry.Mokatebe.TarikhDabirKhaneMarkazi = ((MokatebeInputDto)request.DataEntryData).TarikhNameNamayande;
                        break;
                    case DataEntryTypeEnum.Tarh:
                        data.DataEntry.Tarh = _mapper.Map(request.DataEntryData as TarhDetailDto, data.DataEntry.Tarh);
                        break;
                    case DataEntryTypeEnum.Layehe:
                        var majors = ((LayeheDetailDto)request.DataEntryData).MajorCommissions;
                        var minors = ((LayeheDetailDto)request.DataEntryData).MinorCommissions;
                        data.DataEntry.Layehe = _mapper.Map<LayeheEntity>(request.DataEntryData as LayeheDetailDto);
                        data.DataEntry.Layehe.MajorCommissions = majors != null ? string.Join(",", majors) : null;
                        data.DataEntry.Layehe.MinorCommissions = minors != null ? string.Join(",", minors) : null;
                        break;
                    case DataEntryTypeEnum.Soval:
                        data.DataEntry.Soval = _mapper.Map(request.DataEntryData as SovalDetailDto, data.DataEntry.Soval);
                        break;
                    case DataEntryTypeEnum.DastoorJalasatComission:
                        data.DataEntry.DastoorJalasatComission = _mapper.Map(request.DataEntryData as DastoorJalasatComissionDetailDto, data.DataEntry.DastoorJalasatComission);
                        break;
                    case DataEntryTypeEnum.EzhaaratResaneee:
                        data.DataEntry.EzhaaratResaneee = _mapper.Map(request.DataEntryData as EzhaaratResaneeeDetailDto, data.DataEntry.EzhaaratResaneee);
                        break;
                    case DataEntryTypeEnum.Loan:
                        //
                        var requestLoanData = request.DataEntryData as LoanDtailDto;
                        var currentLoanStatus = data.DataEntry.Loan.VaziatPasokh;
                        var isAdmin = cuser.Roles.Any(r => r == RoleTypeEnum.Admin);
                        var isOrganization = cuser.Roles.Any(r => r == RoleTypeEnum.Organization);


                        //  ادمین فقط نمی‌تواند درخواست رد شده را ویرایش کند
                        if (isAdmin)
                        {
                            if (currentLoanStatus == ResponseStatusEnum.Manfi)
                            {
                                throw new InvalidOperationException("تسهیلات رد شده قابل ویرایش نمی‌باشد.");
                            }
                            // ادمین از بقیه شرط‌ها معاف است
                            
                        }

                        //  بررسی ارجاع به بانک (برای غیر بانک و غیر ادمین)
                        if (!isOrganization && !isAdmin)
                        {
                            var isReferencedToBank = await _context.ActionReferences
                                .AnyAsync(a =>
                                    a.ToUser.OrganizationId != null &&
                                    a.DataEntryId == data.DataEntryId
                                );

                            if (isReferencedToBank)
                            {
                                throw new InvalidOperationException("تسهیلات انتخابی به بانک ارجاع شده است و قابل ویرایش نمی‌باشد.");
                            }

                            //  عدم اجازه ویرایش مجدد برای کاربران غیر بانک
                            if (currentLoanStatus != ResponseStatusEnum.Inprogress)
                            {
                                throw new InvalidOperationException("تسهیلات انتخابی قابل ویرایش مجدد نمی‌باشد.");
                            }
                        }

                        // بررسی ویرایش برای کاربر بانک
                        if (isOrganization)
                        {
                            // بانک نمی‌تواند درخواست‌های تایید یا رد شده را ویرایش کند
                            if (currentLoanStatus == ResponseStatusEnum.Mosbat ||
                                currentLoanStatus == ResponseStatusEnum.Manfi)
                            {
                                throw new InvalidOperationException("تسهیلات انتخابی قابل ویرایش مجدد نمی‌باشد.");
                            }

                            // بانک نمی‌تواند وضعیت شعبه را به در حال بررسی برگرداند
                            if (currentLoanStatus == ResponseStatusEnum.Shobe &&
                                requestLoanData.PasokhState == ResponseStatusEnum.Inprogress)
                            {
                                throw new InvalidOperationException("وضعیت تسهیلات انتخابی قابل بازگشت به قبل نمی‌باشد.");
                            }
                        }


                       
                       
                        requestLoanData.Amount = requestLoanData.Amount.Replace(",", "");
                        if (data.DataEntry.Loan.Amount != long.Parse(requestLoanData.Amount)) // mablagh taghir karde to edit
                        {

                            var persianCalendar = new PersianCalendar();
                            var now = DateTime.Now;
                            var currentPersianYear = persianCalendar.GetYear(now);
                            var startOfPersianYear = new DateTime(currentPersianYear, 1, 1, persianCalendar);
                            var endOfPersianYear = new DateTime(currentPersianYear, 12, 29, 23, 59, 59, persianCalendar);

                            // checking saghf haye etebari
                            var yearlyLoans = _context.Loans.Include(l => l.DataEntry)
                           .Where(x => x.DataEntry.SenatorId == data.DataEntry.SenatorId
                                   && x.DataEntry.Created >= startOfPersianYear
                                   && x.DataEntry.Created <= endOfPersianYear);
                            //if (todayLoans.Count() >= int.Parse(_configuration.GetSection("DailyLoanCount").Value))
                            //{
                            //    throw new InvalidOperationException("شما قادر به معرفی بیش از 2 نفر جهت اخذ تسهیلات در روز نمی‌باشید.");
                            //}
                            //phase 2

                            var havemaxLoaninYearRequestConfig = long.TryParse(_configuration["maxLoanInYearRequest"], out long maxLoanInYearRequest);
                            if (!havemaxLoaninYearRequestConfig) { maxLoanInYearRequest = 5000000000; } // پنج میلیارد تومن در سال

                            if (yearlyLoans.Sum(x => x.Amount) >= maxLoanInYearRequest)
                            {
                                throw new InvalidOperationException("سقف مجاز سالیانه شما جهت معرفی تسهیلات به پایان رسیده‌است.");
                            }
                        }


                        var actRef = new ActionReferenceEntity()
                        {
                            FromUserId = cuser.BussinessUserId,
                            ToUserId = cuser.BussinessUserId,
                            ActRefType = ActRefTypeEnum.Edit,
                            DataEntry = data.DataEntry,
                            Description = $" درخواست از وضعیت {data.DataEntry.Loan.VaziatPasokh.GetPersianName()} به وضعیت {requestLoanData.PasokhState.GetPersianName()} توسط کاربر {cuser.Name} تغییر یافت."
                        };
                        _context.ActionReferences.Add(actRef);


                        data.DataEntry.Loan = _mapper.Map(requestLoanData, data.DataEntry.Loan);
                        data.DataEntry.Loan.VaziatPasokh = requestLoanData.PasokhState;

                        data.DataEntry.Loan.LastModifiedDate = DateTime.Now;
                        data.DataEntry.Loan.LastModifiedUserId = cuser.BussinessUserId;


                        break;
                    case DataEntryTypeEnum.TahghighTafahos:
                        data.DataEntry.TahghighTafahos = _mapper.Map<TahghighTafahosEntity>(request.DataEntryData as TahghighTafahosDetailDto);
                        var senatorIds = ((TahghighTafahosDetailDto)request.DataEntryData).TahghighTafahosSenatorsId;
                        var currentSenators = await _unitOfWork.TahghighTafahosSenatorReposity
                            .NoTracking
                            .Where(r => r.TahghighTafahosId == data.DataEntryId)
                            .Select(x => new Guid(
                                x.SenatorId.ToString())
                            ).ToListAsync(cancellationToken);
                        var mustAdd = senatorIds?.Except(currentSenators);
                        var mustRemove = senatorIds != null ? currentSenators?.Except(senatorIds) : currentSenators;
                        var tahghighTafahos = await _unitOfWork.TahghighTafahosReposity
                            .Tracking
                            .Include(x => x.TahghighTafahosSenators)
                            .Where(x => x.DataEntryId == data.DataEntryId)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(cancellationToken);
                        if (mustRemove?.Count() > 0)
                        {
                            foreach (var item in mustRemove)
                            {
                                await _unitOfWork.TahghighTafahosReposity.RemoveSenatorAsync(data.DataEntryId, item);
                            }
                        }

                        if (mustAdd?.Count() > 0)
                        {
                            foreach (var item in mustAdd)
                            {
                                await _unitOfWork.TahghighTafahosReposity.AddSenatorAsync(data.DataEntry.TahghighTafahos, item);
                            }
                        }
                        break;
                    case DataEntryTypeEnum.Molaghat:
                        data.DataEntry.Molaghat = _mapper.Map(request.DataEntryData as MolaghatDtailDto, data.DataEntry.Molaghat);
                        break;
                    default:
                        throw new InvalidOperationException("نوع دیتا انتری تعریف نشده است");
                }
            }
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var a = ex;
            }


        }
    }
}
