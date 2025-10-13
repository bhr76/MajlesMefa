namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;

public class SendSmsResponse
{
    public ErrorModel ErrorModel { get; set; }
}

public class ErrorModel
{
    public int ErrorCode { get; set; }
}