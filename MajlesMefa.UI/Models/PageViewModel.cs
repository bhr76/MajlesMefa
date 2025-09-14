using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateMenuCommand;

namespace MajlesMefa.UI.Models
{
    public class PageVm 
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "URL")] 
        public string Url { get; set; }

        [Display(Name = "controller")] 
        public string Controller { get; set; }

        [Display(Name = "action")]
        public string Action { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public List<RoleTypeEnum> Roles { get; set; }


        public UpdateMenuCommand ConvertToCommand()
        {
            return new UpdateMenuCommand()
            {
                //PageId = new Guid(),
                //Roles = Roles,
                //Title = Title,
                //Description = Description,
                //DataEntryType = DataEntryTypeEnum.Soval,
            };
        }

        public UpdateMenuCommand ConvertToUpdateCommand()
        {
            return new UpdateMenuCommand()
            {
                PageId= Id,
                Roles = Roles,
            };
        }


    }
}