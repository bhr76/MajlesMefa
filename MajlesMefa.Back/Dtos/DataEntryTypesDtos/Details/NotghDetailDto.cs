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
    public class NotghDetailDto: IMapping, IModifyDataEntryDto
    {
        public DateTime JalaseAlaniDate { get; set; }

        public string Chekide { get; set; }
        public string PasokhNo { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public ResponseStatusEnum PasokhState { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public int PasokhStateInt { get { return (int)PasokhState; } set { PasokhState = (ResponseStatusEnum)value; } }
        public DateTime PasokhDate { get; set; }
        public string GardeshErjaat { get; set; }

        public string AnswerFromProUnitNo { get; set; }
        public DateTime AnswerFromProUnitDate { get; set; }

        public List<PeygiryDto> Peygiries { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotghDetailDto, NotghEntity>()
                .ReverseMap();
        }
    }
}
