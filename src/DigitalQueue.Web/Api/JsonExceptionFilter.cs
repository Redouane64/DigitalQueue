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
        if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            return;
        }
        
        ErrorDto error;
        if (_env.IsDevelopment())
        {
            error = new ErrorDto(context.Exception.Message, context.Exception.StackTrace);
        }
        else
        {
            error = new ErrorDto("Something went wrong.");
        }

        context.Result = new ObjectResult(error) { StatusCode = 500 };
    }
}

