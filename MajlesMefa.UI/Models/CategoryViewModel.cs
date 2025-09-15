using AutoMapper;
using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.UseCases.Commmands.CreateMokatebePeygiriCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateMokatebePeygiriCommand;
using MajlesMefa.Back.UseCases.Commmands.CreateCategoryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateCategoryCommand;

namespace MajlesMefa.UI.Models
{
    public class CategoryVm 
    {

        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "نیاز به فیلد مبلغ دارد؟")]
        public bool HasAmount { get; set; }

        [Display(Name = "آیا زیرمجموعه ادارات کل استان‌ها می‌باشد؟")]
        public bool IsCentralOffice { get; set; }

        [Display(Name = "آیا زیرمجموعه ادارات کل استان‌ها می‌باشد؟")]
        public string IsCentralOfficeStr => IsCentralOffice ? "بله" : "خیر";

        [Display(Name = "پدر")]
        public Guid? ParentId { get; set; }

        [Display(Name = "پدر")]
        public virtual CategoryEntity Parent { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        [Display(Name = "نوع دیتا")]
        public DataEntryTypeEnum? DataEntryType { get; set; }

        public CreateCategoryCommand ConvertToCommand()
        {
            return new CreateCategoryCommand()
            {
                Name = Name,
                ParentId = ParentId,
                DataEntryType = DataEntryType,
                IsCentralOffice= IsCentralOffice,
            };
        }

        public UpdateCategoryCommand ConvertToUpdateCommand()
        {
            return new UpdateCategoryCommand()
            {
                Id = Id,
                Name = Name,
                ParentId = ParentId,
                DataEntryType = DataEntryType,
                IsCentralOffice= IsCentralOffice,
            };
        }


    }
}