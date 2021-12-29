using DigitalQueue.Web.Users;

namespace DigitalQueue.Web.Extensions;

public static class ApplicationExtensions
{
    public static async Task InitializeSeedData(this WebApplication app)
    {
        using (var serviceScope = app.Services.CreateScope())
        {
            var userService = serviceScope.ServiceProvider.GetRequiredService<UsersService>();

            await userService.CreateDefaultAccountsAndRoles();
        }
    }
}
