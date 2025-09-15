using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.AddOrganizationCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateOrganizationCommmand;

namespace MajlesMefa.UI.Models
{
    public class OrganizationVm 
    {

        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(256, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "پدر")]
        public Guid? ParentId { get; set; }

        [Display(Name = "پدر")]
        public string ParentName { get; set; }


        public AddOrganizationCommand ConvertToCommand()
        {
            return new AddOrganizationCommand()
            {
                Name = Name,
                ParentId = ParentId,
            };
        }

        public UpdateOrganizationCommmand ConvertToUpdateCommand()
        {
            return new UpdateOrganizationCommmand()
            {
                Id = Id,
                Name = Name,
                ParentId = ParentId,
            };
        }


    }
}