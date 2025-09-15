using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class TarhDetailDto: IMapping, IModifyDataEntryDto
    {
        public string Shenase { get; set; }

        public string NamayandeghanEmzaKonande { get; set; }

        public Guid RelatedComission { get; set; }

        public string Gardeshkar { get; set; }

        public string HamahangiBaraSherkat { get; set; }

        public string NamayandeghanBaraSherkat { get; set; }

        public string GhozareshMozakerat { get; set; }

        public NatijeBarresiEnum NatijeBarresi { get; set; }

        public VazeyatBarresiEnum VazeyatBarresi { get; set; }

        public string NazarNamayande { get; set; }

        public NatijeBarresiEnum Kollyat { get; set; }

        public NatijeBarresiEnum MavadTarh { get; set; }

        public NatijeBarresiEnum NatijeBarresiShoraNegahban { get; set; }

        public string NatijeBarresiShoraNegahbanDescription { get; set; }

        public string SavabeghEblagh { get; set; }

        public string GhozarshNahayi { get; set; }

        public string Havashi { get; set; }
        
        public string ErsalBeVazir { get; set; }

        public List<KeywordDto> Keywords { get; set; } = new List<KeywordDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TarhEntity, TarhDetailDto>()
                .ReverseMap();
        }
    }
}
