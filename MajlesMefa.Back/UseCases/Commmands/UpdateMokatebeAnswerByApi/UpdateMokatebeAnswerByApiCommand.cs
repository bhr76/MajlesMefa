using MediatR;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.UseCases.Commmands.AddMokatebeAnswerByApi
{
    public class UpdateMokatebeAnswerByApiCommand : IRequest<Guid>
    {
        public long Id { get; set; }

        public Guid MokatebeTraceId { get; set; }

        public string AnswerNumber { get; set; }

        public int AnswerStatusInt { get; set; }

        public DateTime AnswerDate { get; set; }

    }
}
