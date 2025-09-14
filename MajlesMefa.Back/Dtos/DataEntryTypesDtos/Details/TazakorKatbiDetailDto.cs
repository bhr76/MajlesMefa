using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class TazakorKatbiDetailDto : IMapping, IModifyDataEntryDto
    {
        public DateTime GheraatSahnDate { get; set; }
        public DateTime PasokhDate { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public ResponseStatusEnum PasokhState { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public int PasokhStateInt { get { return (int)PasokhState; } set { PasokhState = (ResponseStatusEnum)value; } }

        public string ShomareName { get; set; }
        public string PasokhNo { get; set; }

        public string GardeshErjaat { get; set; }

        public string GardeshErjaatMoavenat { get; set; }

        public DateTime PishnevisDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TazakorKatbiDetailDto, TazakorKatbiEntity>()
                .ReverseMap();
        }
    }
}
