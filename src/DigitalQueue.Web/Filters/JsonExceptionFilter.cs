using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalQueue.Web.Filters;

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
            if (context.Exception.InnerException is not null)
            {
                error = new ErrorDto(context.Exception.InnerException.Message, context.Exception.StackTrace!);
            }
            else
            {
                error = new ErrorDto(context.Exception.Message, context.Exception.StackTrace!);
            }
        }
        else
        {
            error = new ErrorDto("Something went wrong.");
        }

        context.Result = new ObjectResult(error) { StatusCode = 500 };
    }
}

