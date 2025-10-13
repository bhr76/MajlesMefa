namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;

public class SendSmsRequest
{
    public SendSmsRequest(string phone, string message)
    {
        Phone = phone;
        Message = message;
    }

    public string Message { get; set; }
    public string Phone { get; set; }
}