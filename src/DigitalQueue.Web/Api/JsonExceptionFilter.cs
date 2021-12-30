using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalQueue.Web.Api;

public class JsonExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment _env;

    public JsonExceptionFilter(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        ErrorViewModel error;
        if (_env.IsDevelopment())
        {
            error = new ErrorViewModel(context.Exception.Message, context.Exception.StackTrace);
        }
        else
        {
            error = new ErrorViewModel("Something went wrong.");
        }

        context.Result = new ObjectResult(error) { StatusCode = 500 };
    }
}

