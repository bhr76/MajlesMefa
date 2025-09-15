using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Api
{
    public class MokatebeDataDto : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }
        public int MokatebeKonandeInt { get; set; }
        public int MokatebeTypeInt { get; set; }
        public int ContactInt { get; set; }
        public string ShomareDabirkhane { get; set; }
        public string ShomareDabirkhaneMarkazi { get; set; }
        public DateTime TarikhDabirKhane { get; set; }
        public string TarikhDabirKhaneStr { get; set; }

        public string PeygiriKonande { get; set; }

        public string PeygiriNumber { get; set; }

        public string PeygiriDescription { get; set; }
        public DateTime PeygiriDate { get; set; }
        public string PeygiriDateStr { get; set; }

        public int VaziatPasokhInt { get; set; }
        public string PasokhNo { get; set; }
        public DateTime PasokhDate { get; set; }
        public string PasokhDateShamsi { get; set; }

        public string PasokhPersianDate => this.PasokhDate.ToPersianDate();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MokatebeDataDto, MokatebeEntity>()
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
            profile.CreateMap<MokatebeDataDto, PeygiriEntity>()
                .ForMember(x => x.Id, s => s.Ignore())
                .ForMember(x => x.Description, s => s.MapFrom(y => y.PeygiriDescription))
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
        }
    }

    public class MokatebeRequestDto
    {

        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string SenatorUUID { get; set; }

        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get; set; }

        public MokatebeInputDto MokatebeData { get; set; }

        public Guid CategoryParentId { get; set; }

        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Description { get; set; }
    }
}
