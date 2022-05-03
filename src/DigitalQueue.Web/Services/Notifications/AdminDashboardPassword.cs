namespace DigitalQueue.Web.Services.Notifications;

public class AdminDashboardPassword
{
    public AdminDashboardPassword(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    public string Password { get; }
}
