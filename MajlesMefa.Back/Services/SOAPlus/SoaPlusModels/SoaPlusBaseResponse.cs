namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;

public class SoaPlusBaseResponse<T>
{
    public SoaPlusBaseResponse(DateTime doneDateTime, bool done, T? result, string? errorMessage)
    {
        DoneDateTime = doneDateTime;
        Done = done;
        Result = result;
        ErrorMessage = errorMessage;
    }

    public DateTime DoneDateTime { get; set; }
    public bool Done { get; set; }
    public T? Result { get; set; }
    public string? ErrorMessage { get; set; }
}