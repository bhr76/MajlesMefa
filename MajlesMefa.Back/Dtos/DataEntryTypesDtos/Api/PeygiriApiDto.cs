using MajlesMefa.Back.UseCases.Commmands.CreateMokatebePeygiriCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateMokatebePeygiriCommand;
using MajlesMefa.Back.Utilities.Date;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Api
{
    public class PeygiriDto
    {
        [Display(Name = "شناسه")]
        public long Id { get; set; }

        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid MokatebeTraceId { get; set; }


        [Display(Name = "پیگیری کننده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid PeygiriKonandeId { get; set; }

        [Display(Name = "شماره پیگیری")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string PeygiriNumber { get; set; }

        [Display(Name = "توضیحات پیگیری")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string PeygiriDescription { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string PeygiriDateShamsi { get; set; }



        public CreateMokatebePeygiriCommand ConvertToCommand()
        {
            return new CreateMokatebePeygiriCommand()
            {
                DataEntryId = MokatebeTraceId,
                Description = PeygiriDescription,
                PeygiriNumber = PeygiriNumber,
                PeygiriKonandeId = PeygiriKonandeId,
                PeygiriDate = PeygiriDateShamsi.ToMiladiDate(),
            };
        }
    }
}
