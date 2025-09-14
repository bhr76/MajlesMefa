using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class DastoorJalasatComissionDetailDto : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "گزارشات")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 5)]
        public string Ghozareshat { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime DateAndDay { get; set; }

        [Display(Name = "تاریخ")]
        public string JalaliTarikh => this.DateAndDay == DateTime.MinValue ? null : this.DateAndDay.ToPersianDate();


        public bool HasRelation { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DastoorJalasatComissionDetailDto, DastoorJalasatComissionEntity>()
                .ReverseMap();
        }
    }
}
