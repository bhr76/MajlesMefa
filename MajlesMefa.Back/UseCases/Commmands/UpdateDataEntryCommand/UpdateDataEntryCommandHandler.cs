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

namespace MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand
{
    public class UpdateDataEntryCommandHandler : IRequestHandler<UpdateDataEntryCommand>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDataEntryCommandHandler(RefahMajlesDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
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

            var actRef = new ActionReferenceEntity()
            {
                FromUserId = cuser.BussinessUserId,
                ToUserId = cuser.BussinessUserId,
                ActRefType = ActRefTypeEnum.Edit,
                DataEntry = data.DataEntry
            };
            _context.ActionReferences.Add(actRef);


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
                        data.DataEntry.Loan = _mapper.Map(request.DataEntryData as LoanDtailDto, data.DataEntry.Loan);
                        data.DataEntry.Loan.VaziatPasokh = (request.DataEntryData as LoanDtailDto).PasokhState;
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
