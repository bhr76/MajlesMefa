namespace MajlesMefa.Back.Utilities.Message;

public static class Message
{
    public static string Show(string message, MessageType type = MessageType.Info)
    {
        var title = type switch
        {
            MessageType.Success => "موفقیت",
            MessageType.Error => "خطا",
            MessageType.Info => "اطلاعات",
            MessageType.Warning => "هشدار",
            _ => string.Empty
        };
        return $"toastr[\"{type.ToString().ToLower()}\"](\"{message}\", \"{title}\");";
    }
}

public enum MessageType
{
    Success,
    Error,
    Info,
    Warning
}