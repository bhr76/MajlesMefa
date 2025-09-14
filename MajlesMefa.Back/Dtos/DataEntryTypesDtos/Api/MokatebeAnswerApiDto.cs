using MajlesMefa.Back.UseCases.Commmands.AddMokatebeAnswerByApi;
using MajlesMefa.Back.UseCases.Commmands.CreateMokatebePeygiriCommand;
using MajlesMefa.Back.Utilities.Date;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Api
{
    public class MokatebeAnswerDto
    {
        [Display(Name = "شناسه")]
        public long Id { get; set; }

        public Guid MokatebeTraceId { get; set; }


        [Display(Name = "شماره پاسخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string AnswerNumber { get; set; }

        [Display(Name = "وضعیت پاسخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int AnswerStatusInt { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string AnswerDateShamsi { get; set; }


        public UpdateMokatebeAnswerByApiCommand ConvertToCommand()
        {
            return new UpdateMokatebeAnswerByApiCommand()
            {
                MokatebeTraceId = MokatebeTraceId,
                AnswerNumber = AnswerNumber,
                AnswerStatusInt = AnswerStatusInt,
                AnswerDate = AnswerDateShamsi.ToMiladiDate(),
            };
        }


    }
}
