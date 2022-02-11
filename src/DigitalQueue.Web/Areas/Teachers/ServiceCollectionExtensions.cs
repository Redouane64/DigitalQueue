using DigitalQueue.Web.Areas.Teachers.Services;

namespace DigitalQueue.Web.Areas.Teachers;

public static class ServiceCollectionExtensions
{
    public static void AddTeachersServices(this IServiceCollection services)
    {
        services.AddScoped<TeachersService>();
    }
}
