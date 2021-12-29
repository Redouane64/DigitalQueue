using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Data;

public static class ServiceCollectionExtensions
{
    public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DigitalQueueContext>(options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();

            options.UseSqlite("Data Source=digital-queue.db");
        });
    }
}
