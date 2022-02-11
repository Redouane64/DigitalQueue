using System.Text.Json.Serialization;

using DigitalQueue.Web.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;

namespace DigitalQueue.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add<JsonExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var error = new ErrorDto("One or more validation problem occurred.");

                    var actionExecutingContext = context as ActionExecutingContext;

                    if (context.ModelState.ErrorCount == 0)
                    {
                        return new BadRequestObjectResult(error)
                        {
                            ContentTypes = { "application/problem+json" },
                        };
                    }

                    var errorMessage = context.ModelState
                        .Select(m => m.Value)
                        .FirstOrDefault();

                    error.Message = errorMessage?.Errors.FirstOrDefault()?.ErrorMessage ?? error.Message;

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

        services.AddCors(options =>
        {
            options.AddPolicy("_AnyClient", 
                builder => builder.WithMethods("GET", "OPTIONS", "POST")
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .SetPreflightMaxAge(TimeSpan.FromDays(5)).Build());
        });
    }
}
