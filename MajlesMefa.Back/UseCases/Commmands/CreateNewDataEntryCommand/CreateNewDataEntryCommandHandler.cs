using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using MajlesMefa.Back.Utilities.Convertor;

namespace MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand
{
    public class CreateNewDataEntryCommandHandler : IRequestHandler<CreateNewDataEntryCommand, Guid>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CreateNewDataEntryCommandHandler(RefahMajlesDbContext context,
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

        public async Task<Guid> Handle(CreateNewDataEntryCommand request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();
            var dataEntity = _mapper.Map<DataEntryEntity>(request);
            dataEntity.CreatorId = cuser.BussinessUserId;
             var actRef = new ActionReferenceEntity()
            {
                FromUserId = cuser.BussinessUserId,
                ToUserId = cuser.BussinessUserId,
                ActRefType = ActRefTypeEnum.Create,
                DataEntry = dataEntity
            };
            //if (request.CategoryId.HasValue)
            //{
            //    var category = await _context.Categories
            //        .Where(c => c.Id == request.CategoryId)
            //        .Select(c => c.DataEntryType)
            //        .SingleOrDefaultAsync(cancellationToken);
            //    if (category.HasValue && category != request.DataEntryType)
            //    {
            //        throw new InvalidOperationException("نوع دسته بندی با نوع موجودیت انتخابی تطابق ندارد");
            //    }
            //}

            dataEntity.CategoryId = request.CategoryId;
            dataEntity.SenatorId = request.SenatorId;
            dataEntity.Moavenats = request.Moavenats != null ? string.Join(",", request.Moavenats) : null;


            _context.Add(dataEntity);
            _context.Add(actRef);

            switch (request.DataEntryType)
            {
                case DataEntryTypeEnum.Notgh:
                    var notghDto = request.DataEntryData as NotghDetailDto;
                    var notgh = _mapper.Map<NotghEntity>(notghDto);
                    notgh.DataEntry = dataEntity;
                    _context.Notghs.Add(notgh);
                    break;
                case DataEntryTypeEnum.Tazakor:
                    var tazakor = _mapper.Map<TazakorEntity>(request.DataEntryData as TazakorDetailDto);
                    tazakor.DataEntry = dataEntity;
                    _context.Tazakors.Add(tazakor);
                    break;
                case DataEntryTypeEnum.TazakorKatbi:
                    var tazakorKatbi = _mapper.Map<TazakorKatbiEntity>(request.DataEntryData as TazakorKatbiDetailDto);
                    tazakorKatbi.DataEntry = dataEntity;
                    _context.TazakorKatbis.Add(tazakorKatbi);
                    break;
                case DataEntryTypeEnum.TazakorShafahi:
                    var tazakorShafahi = _mapper.Map<TazakorShafahiEntity>(request.DataEntryData as TazakorShafahiDetailDto);
                    tazakorShafahi.DataEntry = dataEntity;
                    _context.TazakorShafahis.Add(tazakorShafahi);
                    break;
                case DataEntryTypeEnum.Loan:
                    var loan = _mapper.Map<LoanEntity>(request.DataEntryData as LoanDtailDto);
                    loan.DataEntry = dataEntity;

                    //persian year
                    var persianCalendar = new PersianCalendar();
                    var now = DateTime.Now;
                    var currentPersianYear = persianCalendar.GetYear(now);
                    var startOfPersianYear = new DateTime(currentPersianYear, 1, 1, persianCalendar);
                    var endOfPersianYear = new DateTime(currentPersianYear, 12, 29, 23, 59, 59, persianCalendar);
                    //
                    var haveOpenLoan = _context.Loans.Include(l => l.DataEntry)
                        .Where(x => x.DataEntry.Loan.NationalNo == loan.NationalNo
                                && x.LoanType == loan.LoanType
                                && x.VaziatPasokh != ResponseStatusEnum.Manfi).Count() > 0;
                    if (haveOpenLoan)
                    {
                        throw new InvalidOperationException("فرد مورد درخواست در حال حاضر وامی بدون تعیین وضعیت در سیستم دارد.‌");
                    }
                    var yearlyLoans = _context.Loans.Include(l => l.DataEntry)
                        .Where(x => x.DataEntry.SenatorId == loan.DataEntry.SenatorId
                                && x.VaziatPasokh != ResponseStatusEnum.Manfi
                                && x.DataEntry.Created >= startOfPersianYear
                                && x.DataEntry.Created <= endOfPersianYear);
                    //if (todayLoans.Count() >= int.Parse(_configuration.GetSection("DailyLoanCount").Value))
                    //{
                    //    throw new InvalidOperationException("شما قادر به معرفی بیش از 2 نفر جهت اخذ تسهیلات در روز نمی‌باشید.");
                    //}
                    //phase 2

                    var havemaxGharzolhasaneInYearRequestConfig = long.TryParse(_configuration["maxGharzolhasaneInYearRequest"], out long maxGharzolhasaneInYearRequest);
                    if (!havemaxGharzolhasaneInYearRequestConfig) { maxGharzolhasaneInYearRequest = 3000000000; } // سه میلیارد تومن در سال
                    if (yearlyLoans.Where(l=>l.LoanType==LoanTypeEnum.Gharzolhasane).Sum(x => x.Amount) >= maxGharzolhasaneInYearRequest)
                    {
                        throw new InvalidOperationException("سقف مجاز سالیانه شما جهت معرفی تسهیلات قرض الحسنه به پایان رسیده‌است.");
                    }

                    var havemaxmaxMorabeheInYearRequestConfig = long.TryParse(_configuration["maxMorabeheInYearRequest"], out long maxMorabeheInYearRequest);
                    if (!havemaxmaxMorabeheInYearRequestConfig) { maxMorabeheInYearRequest = 3000000000; } // سه میلیارد تومن در سال
                    if (yearlyLoans.Where(l => l.LoanType == LoanTypeEnum.Morabehe).Sum(x => x.Amount) >= maxMorabeheInYearRequest)
                    {
                        throw new InvalidOperationException("سقف مجاز سالیانه شما جهت معرفی تسهیلات مرابحه به پایان رسیده‌است.");
                    }
                    loan.NationalNo = PersianToEnglish.ConvertPersianToEnglishNumber(loan.NationalNo);
                    loan.MobileNo = PersianToEnglish.ConvertPersianToEnglishNumber(loan.MobileNo);
                    _context.Loans.Add(loan);
                    break;
                case DataEntryTypeEnum.Mokatebe:
                    var mokatebeDto = request.DataEntryData as MokatebeInputDto;
                    var mokatebe = _mapper.Map<MokatebeEntity>(mokatebeDto);
                    mokatebe.TarikhDabirKhaneMarkazi = ((MokatebeInputDto)request.DataEntryData).TarikhNameNamayande;
                    var isShomareNameRepeated = _context.Mokatebes.FirstOrDefault(x => x.ShomareDabirkhane == mokatebeDto.ShomareDabirkhane);
                    if (isShomareNameRepeated != null)
                    {
                        throw new InvalidOperationException("شماره نامه نمیتواند تکراری باشد.");
                    }
                    _context.Mokatebes.Add(mokatebe);
                    mokatebe.DataEntry = dataEntity;
                    if (mokatebeDto.HasPeygiry)
                    {
                        var peygiry = _mapper.Map<PeygiriEntity>(mokatebeDto);
                        peygiry.DataEntry = dataEntity;
                        peygiry.PeygiriKonandeId = new Guid(mokatebeDto.PeygiriKonande);
                        _context.Peygiries.Add(peygiry);
                    }
                    break;
                case DataEntryTypeEnum.Tarh:
                    var tarhDto = request.DataEntryData as TarhDetailDto;
                    var tarh = _mapper.Map<TarhEntity>(tarhDto);
                    _context.Tarhs.Add(tarh);
                    tarh.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.Layehe:
                    var layeheDto = request.DataEntryData as LayeheDetailDto;
                    var layehe = _mapper.Map<LayeheEntity>(layeheDto);
                    var majors = layeheDto.MajorCommissions;
                    var minors = layeheDto.MinorCommissions;
                    layehe.MajorCommissions = majors != null ? string.Join(",", majors) : null;
                    layehe.MinorCommissions = minors != null ? string.Join(",", minors) : null;

                    _context.Layehes.Add(layehe);
                    layehe.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.Soval:
                    var sovalDto = request.DataEntryData as SovalDetailDto;
                    var soval = _mapper.Map<SovalEntity>(sovalDto);
                    _context.Sovals.Add(soval);
                    soval.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.EzhaaratResaneee:
                    var ezhaaratResaneeeDto = request.DataEntryData as EzhaaratResaneeeDetailDto;
                    var ezhaaratResaneee = _mapper.Map<EzhaaratResaneeeEntity>(ezhaaratResaneeeDto);
                    _context.EzhaaratResaneeees.Add(ezhaaratResaneee);
                    ezhaaratResaneee.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.DastoorJalasatComission:
                    var dastoorJalasatComissionDto = request.DataEntryData as DastoorJalasatComissionDetailDto;
                    var dastoorJalasatComission = _mapper.Map<DastoorJalasatComissionEntity>(dastoorJalasatComissionDto);
                    _context.DastoorJalasatComissions.Add(dastoorJalasatComission);
                    dastoorJalasatComission.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.TahghighTafahos:
                    var tahghighTafahosDto = request.DataEntryData as TahghighTafahosDetailDto;

                    var tahghighTafahos = _mapper.Map<TahghighTafahosEntity>(tahghighTafahosDto);

                    var senatorIds = ((TahghighTafahosDetailDto)request.DataEntryData).TahghighTafahosSenatorsId;
                    if (senatorIds?.Count > 0)
                    {
                        foreach (var item in senatorIds)
                        {
                            await _unitOfWork.TahghighTafahosReposity.AddSenatorAsync(tahghighTafahos, item);
                        }
                    }

                    _context.TahghighTafahoses.Add(tahghighTafahos);
                    tahghighTafahos.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.Molaghat:
                    var molaghatDto = request.DataEntryData as MolaghatDtailDto;
                    var molaghat = _mapper.Map<MolaghatEntity>(molaghatDto);
                    _context.Molaghats.Add(molaghat);
                    molaghat.DataEntry = dataEntity;
                    break;
                case DataEntryTypeEnum.Khadamat:
                    var khadamatDto = request.DataEntryData as KhadamatDetailDto;
                    var khadamat =_mapper.Map<KhadamatEntity>(khadamatDto);
                    _context.Khadamat.Add(khadamat);
                    khadamat.DataEntry = dataEntity;
                    break;
                default:
                    throw new InvalidOperationException("نوع دیتا انتری تعریف نشده است");
            }
           
            await _context.SaveChangesAsync(cancellationToken);
            
            return dataEntity.Id;
        }
    }
}
