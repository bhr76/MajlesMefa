using MajlesMefa.Back.UseCases.Commmands.CreateMokatebePeygiriCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateMokatebePeygiriCommand;
using MajlesMefa.Back.Utilities.Date;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class PeygiriVm
    {
        [Display(Name = "شناسه")]
        public long Id { get; set; }

        public Guid DataEntryId { get; set; }

        [Display(Name = "پیگیری کننده")]
        public string PeygiriKonandeTitle { get; set; }

        [Display(Name = "پیگیری کننده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid PeygiriKonandeId { get; set; }

        [Display(Name = "شماره پیگیری")]
        public string PeygiriNumber { get; set; }

        [Display(Name = "توضیحات پیگیری")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string PeygiriDescription { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime PeygiriDate { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string PeygiriDatePicker { get; set; }

        public string PeygiriDateStr => this.PeygiriDate.ToPersianDate();


        public CreateMokatebePeygiriCommand ConvertToCommand()
        {
            return new CreateMokatebePeygiriCommand()
            {
                DataEntryId = DataEntryId,
                Description = PeygiriDescription,
                PeygiriNumber = PeygiriNumber,
                PeygiriKonandeId = PeygiriKonandeId,
                PeygiriDate = PeygiriDatePicker.ToMiladiDate(),
            };
        }

        public UpdateMokatebePeygiriCommand ConvertToUpdateCommand()
        {
            return new UpdateMokatebePeygiriCommand()
            {
                Id = Id,
                Description = PeygiriDescription,
                PeygiriNumber = PeygiriNumber,
                PeygiriKonandeId = PeygiriKonandeId,
                PeygiriDate = PeygiriDatePicker.ToMiladiDate(),
            };
        }
    }
}
