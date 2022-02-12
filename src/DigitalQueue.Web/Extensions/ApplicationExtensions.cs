using DigitalQueue.Web.Areas.Accounts.Services;
using DigitalQueue.Web.Data;

namespace DigitalQueue.Web.Extensions;

public static class ApplicationExtensions
{
    public static async Task InitializeDefaultUser(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var userService = serviceScope.ServiceProvider.GetRequiredService<UsersService>();
        var defaultUser = app.Configuration.GetSection("DefaultUser");

        if (defaultUser is null)
        {
            return;
        }

        string email = defaultUser.GetValue<string>("Email");
        string password = defaultUser.GetValue<string>("Password");

        await userService.CreateUser(email, password, RoleDefaults.Administrator);
    }
}
