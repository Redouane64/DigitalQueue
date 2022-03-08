using DigitalQueue.Web.Areas.Accounts.Commands;
using DigitalQueue.Web.Data;

using MediatR;

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

        await mediator.Send(new CreateAccountCommand(
            new[] { RoleDefaults.Administrator, RoleDefaults.User }, 
            isActive: true)
        {
            Email = email,
            Password = password,
            FullName = "Admin",
        });
    }
}
