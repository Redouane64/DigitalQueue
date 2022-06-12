using DigitalQueue.Web.Areas.Accounts.Dtos;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class CreateUserAuthenticationTokenCommand : IRequest<AuthenticationStatusDto?>
{
    public string Email { get; }
    public string? DeviceToken { get; }

    public CreateUserAuthenticationTokenCommand(string email, string? deviceToken = null)
    {
        Email = email;
        DeviceToken = deviceToken;
    }
}
