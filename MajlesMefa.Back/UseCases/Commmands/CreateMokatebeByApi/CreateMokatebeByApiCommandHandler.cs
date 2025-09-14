using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Api;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;

namespace MajlesMefa.Back.UseCases.Commmands.CreateMokatebeByApi
{
    public class CreateMokatebeByApiCommandHandler : IRequestHandler<CreateMokatebeByApiCommand, Guid>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CreateMokatebeByApiCommandHandler(RefahMajlesDbContext context, IMapper mapper, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Guid> Handle(CreateMokatebeByApiCommand request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();

            var senator = _context.SenatorProfiles.FirstOrDefault(x => x.SenatorUID == request.SenatorUUID);
            if(senator == null) { throw new ArgumentException("نماینده یافت نشد."); }


            //var dataEntryEntity= new DataEntryEntity
            //{
            //    Title = request.Title,
            //    Description= request.Description,
            //    CategoryId = request.CategoryParentId,
            //    Created = new DateTime().Date,

            //}

            request.SenatorId = senator.UserId;

            var dataEntity = _mapper.Map<DataEntryEntity>(request);
            
            dataEntity.CreatorId = cuser.BussinessUserId;
            var actRef = new ActionReferenceEntity()
            {
                FromUserId = cuser.BussinessUserId,
                ToUserId = cuser.BussinessUserId,
                ActRefType = ActRefTypeEnum.Create,
                DataEntry = dataEntity
            };
            //var senatorEntity = await _context.SenatorProfiles.FirstOrDefault(x=>x.)
            //dataEntity.CategoryId = request.CategoryId;
            //dataEntity.SenatorId = request.SenatorId;
            //dataEntity.Moavenats = request.Moavenats != null ? string.Join(",", request.Moavenats) : null;
            dataEntity.CategoryId = Guid.Parse(_configuration.GetSection("TaminCategoryId").Value);
            dataEntity.DataEntryType = DataEntryTypeEnum.Mokatebe;

            _context.Add(dataEntity);
            _context.Add(actRef);


            var mokatebeDto = request.DataEntryData as MokatebeInputDto;
            if (mokatebeDto.VaziatPasokhInt == 0)
            {
                mokatebeDto.PasokhNo = null;
                mokatebeDto.PasokhDate = default;
            }
            var mokatebe = _mapper.Map<MokatebeEntity>(mokatebeDto);
            mokatebe.TarikhDabirKhaneMarkazi = ((MokatebeInputDto)request.DataEntryData).TarikhNameNamayande;
            mokatebe.DataEntryId = dataEntity.Id;
            //var mokatebe1 = new MokatebeEntity()
            //{
            //    DataEntryId = dataEntity.Id,
            //    ShomareDabirkhane = mokatebeDto.ShomareDabirkhane
            //}
            var isShomareNameRepeated = _context.Mokatebes.FirstOrDefault(x => x.ShomareDabirkhane == mokatebeDto.ShomareDabirkhane);
            if (isShomareNameRepeated != null)
            {
                throw new ArgumentException("شماره نامه نمیتواند تکراری باشد.");
            }
            

            _context.Mokatebes.Add(mokatebe);
            mokatebe.DataEntry = dataEntity;

            if (!string.IsNullOrEmpty(mokatebeDto.PeygiriNumber) && !string.IsNullOrEmpty(mokatebeDto.PeygiriDateShamsi))
            {
                var peygiry = _mapper.Map<PeygiriEntity>(mokatebeDto);
                peygiry.DataEntry = dataEntity;
                peygiry.PeygiriKonandeId = Guid.Parse(_configuration.GetSection("TaminPeygiriKonandeId").Value);
                _context.Peygiries.Add(peygiry);
            }

            await _context.SaveChangesAsync(cancellationToken);



            return dataEntity.Id;
        }
    }
}
