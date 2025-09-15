using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class EzhaaratResaneeeDetailDto : IMapping, IModifyDataEntryDto
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "منبع")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Manba { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ")]
        public string JalaliTarikh => this.Date.ToPersianDate();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EzhaaratResaneeeDetailDto, EzhaaratResaneeeEntity>()
                .ReverseMap();
        }
    }
}
