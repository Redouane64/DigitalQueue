namespace DigitalQueue.Web.Filters;

public class ErrorDto
{
    public ErrorDto(string message)
    {
        Message = message;
    }

    public ErrorDto(string message, string stacktrace)
        : this(message)
    {
        StackTrace = stacktrace;
    }

    public string Message { get; set; }
    
    public string StackTrace { get; set; }
}

