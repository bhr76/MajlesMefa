using AutoMapper;
using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using Microsoft.AspNetCore.Mvc.Rendering;
using MajlesMefa.Back.Migrations;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;

namespace MajlesMefa.UI.Models
{
    public class LoanVm 
    {
        
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public Guid SenatorId { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        
        public LoanDtailDto LoanData { get; set; }

        [ Display(Name = "کاربر جهت ارجاع")]
        public Guid? UserId { get; set; }
        public SelectList UserSelectList { get; set; }

        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = LoanData,
                Title = "",
                CategoryId = null,
                Moavenats = null,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Loan,
                SenatorId = SenatorId,
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = LoanData,
                Title = "",
                CategoryId = null,
                Moavenats = null,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Loan,
                SenatorId = SenatorId,
            };
        }

        public class NotEmptyGuid : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || (Guid)value == Guid.Empty)
                {
                    return new ValidationResult("پر کردن این فیلد اجباری است");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }


    }
}