using AutoMapper;
using MediatR;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;

namespace MajlesMefa.UI.Models
{
    public class CommissionVm 
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "زیر موضوع")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "تاریخ")]
        public string Date { get; set; }


        public DastoorJalasatComissionDetailDto CommissionData { get; set; }


        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = CommissionData,
                CategoryId = CategoryId,
                Title = Title,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission,
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = CommissionData,
                CategoryId = CategoryId,
                Title = Title,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission,
            };
        }


    }
}