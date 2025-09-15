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
    public class TazakorDetailDto : IMapping, IModifyDataEntryDto
    {
        public DateTime GheraatSahnDate { get; set; }

        public string ShomareName { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public ResponseStatusEnum PasokhState { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public int PasokhStateInt { get { return (int)PasokhState; } set { PasokhState = (ResponseStatusEnum)value; } }

        [Display(Name = "نوع تذکر")]
        public TazakorEnum TazakorType { get; set; }

        [Display(Name = "نوع تذکر")]
        public int TazakorTypeInt { get { return (int)TazakorType; } set { TazakorType = (TazakorEnum)value; } }

        public string GardeshErjaat { get; set; }

        public string GardeshErjaatMoavenat { get; set; }
        public string PasokhNo { get; set; }

        public DateTime PishnevisDate { get; set; }
        public DateTime PasokhDate { get; set; }

        public DateTime NameVaseleDate { get; set; }
        public string NameVaseleNo { get; set; }
        public string NameVaseleDabirkhaneNo { get; set; }
        public VaseleAzEnum VaseleAz { get; set; }
        public int VaseleAzInt { get { return (int)VaseleAz; } set { VaseleAz = (VaseleAzEnum)value; } }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TazakorDetailDto, TazakorEntity>()
                .ReverseMap();
        }
    }
}
