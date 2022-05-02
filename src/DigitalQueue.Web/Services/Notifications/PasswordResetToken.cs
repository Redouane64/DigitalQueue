namespace DigitalQueue.Web.Services.Notifications;

public class PasswordResetToken
{
    public PasswordResetToken(string Email, string Token)
    {
        this.Email = Email;
        this.Token = Token;
    }

    public string Email { get; init; }
    public string Token { get; init; }

    public void Deconstruct(out string Email, out string Token)
    {
        Email = this.Email;
        Token = this.Token;
    }
}
