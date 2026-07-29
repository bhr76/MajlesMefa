using MediatR;

namespace MajlesMefa.Back.UseCases.Commmands.LoanBatchCommand
{
    public record BatchChangeStatusCommand(BatchChangeStatusDto Dto) : IRequest<bool>;

    public record BatchRegisterActionCommand(BatchRegisterActionDto Dto) : IRequest<bool>;
}
