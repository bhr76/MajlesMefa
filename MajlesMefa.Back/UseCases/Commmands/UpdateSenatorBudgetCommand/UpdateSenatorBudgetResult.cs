namespace MajlesMefa.Back.UseCases.Commmands.UpdateSenatorBudgetCommand
{
    public sealed class UpdateSenatorBudgetResult
    {
        public bool Status { get; init; }
        public string Reason { get; init; }

        public UpdateSenatorBudgetResult(bool status, string reason)
        {
            Status = status;
            Reason = reason;
        }

        public static UpdateSenatorBudgetResult Success(string? reason = null) =>
            new(true, reason ?? string.Empty);

        public static UpdateSenatorBudgetResult Fail(string reason) =>
            new(false, reason);
    }
}