using DigitalQueue.Web.Areas.Accounts.Commands.Authentication;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Extensions;

public static class ApplicationExtensions
{
    public static async Task InitializeDefaultUser(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        var defaultUser = app.Configuration.GetSection("DefaultUser");

        if (defaultUser is null)
        {
            return;
        }

        string email = defaultUser.GetValue<string>("Email");
        string password = defaultUser.GetValue<string>("Password");
        await mediator.Send(new CreateDefaultAccountCommand(email, password));
    }

    public static async Task ApplyMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DigitalQueueContext>();
        await dbContext.Database.MigrateAsync();
    }
}
