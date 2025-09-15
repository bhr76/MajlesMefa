using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.AddCityCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateCityCommand;

namespace MajlesMefa.UI.Models
{
    public class CityVm 
    {

        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "پدر")]
        public Guid? ParentId { get; set; }

        [Display(Name = "پدر")]
        public string ParentName { get; set; }


        public AddCityCommand ConvertToCommand()
        {
            return new AddCityCommand()
            {
                Name = Name,
                ParentCityId = ParentId,
            };
        }

        public UpdateCityCommand ConvertToUpdateCommand()
        {
            return new UpdateCityCommand()
            {
                Id = Id,
                Name = Name,
                ParentCityId = ParentId,
            };
        }


    }
}