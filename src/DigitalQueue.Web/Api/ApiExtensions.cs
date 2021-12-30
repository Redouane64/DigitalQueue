using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;

namespace DigitalQueue.Web.Api;

public static class ApiExtensions
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add<JsonExceptionFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    ErrorViewModel error = new ErrorViewModel("One or more validation problem occurred.");

                    var actionExecutingContext = context as ActionExecutingContext;

                    if (context.ModelState.ErrorCount <= 0 ||
                        (actionExecutingContext?.ActionArguments.Count !=
                         context.ActionDescriptor.Parameters.Count))
                    {
                        return new BadRequestObjectResult(error)
                        {
                            ContentTypes = { "application/problem+json" },
                        };
                    }

                    var errorMessage = context.ModelState
                        .Select(m => m.Value)
                        .FirstOrDefault();

                    error.Message = errorMessage.Errors.FirstOrDefault()?.ErrorMessage;

                    return new BadRequestObjectResult(error)
                    {
                        ContentTypes = { "application/problem+json" },
                    };

                };
            });


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Digital Queue API", Version = "v1" });
        });

    }
}
