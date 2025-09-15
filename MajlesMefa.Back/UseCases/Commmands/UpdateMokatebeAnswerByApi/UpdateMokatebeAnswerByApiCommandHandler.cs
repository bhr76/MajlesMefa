using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.UseCases.Commmands.AddMokatebeAnswerByApi
{
    public class UpdateMokatebeAnswerByApiCommandHandler : IRequestHandler<UpdateMokatebeAnswerByApiCommand, Guid>
    {
        private readonly RefahMajlesDbContext _context;

        public UpdateMokatebeAnswerByApiCommandHandler(RefahMajlesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(UpdateMokatebeAnswerByApiCommand request, CancellationToken cancellationToken)
        {
            var mokatebe = _context.Mokatebes.Where(m => m.DataEntryId == request.MokatebeTraceId).FirstOrDefault();
            if (mokatebe == null) { throw new ArgumentException("مکاتبه یافت نشد."); }

            mokatebe.PasokhNo = request.AnswerNumber;
            mokatebe.PasokhDate = request.AnswerDate;
            mokatebe.VaziatPasokh = (ResponseStatusEnum)request.AnswerStatusInt;


            if (request.AnswerStatusInt == 0)
            {
                mokatebe.PasokhNo = null;
                mokatebe.PasokhDate = default;
                mokatebe.VaziatPasokh = default;
            }

            _context.Update(mokatebe);
            await _context.SaveChangesAsync(cancellationToken);

            return mokatebe.DataEntryId;
        }
    }
}
