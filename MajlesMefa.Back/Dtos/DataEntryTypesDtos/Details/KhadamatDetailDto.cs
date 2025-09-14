using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class KhadamatDetailDto : IMapping, IModifyDataEntryDto
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }
        [Display(Name ="خدمت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Khedmat { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]

        public DateTime TarikhKhedmat { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string JalaliTarikh => this.TarikhKhedmat.ToPersianDate();

        public void Mapping(Profile profile)
        {

            profile.CreateMap<KhadamatDetailDto, KhadamatEntity>()
                .ReverseMap();
        }
    }
}
