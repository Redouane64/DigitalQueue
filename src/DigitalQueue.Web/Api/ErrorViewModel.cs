namespace DigitalQueue.Web.Api;

public class ErrorViewModel
{
    public ErrorViewModel(string message)
    {
        Message = message;
    }

    public ErrorViewModel(string message, string stacktrace)
        : this(message)
    {
        StackTrace = stacktrace;
    }

    public string Message { get; set; }

    public string StackTrace { get; set; }
}

