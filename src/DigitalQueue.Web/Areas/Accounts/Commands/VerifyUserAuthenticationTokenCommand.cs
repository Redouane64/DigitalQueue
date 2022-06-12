using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class VerifyUserAuthenticationTokenCommand : IRequest<AuthenticationResultDto?>
{
    public string Email { get; }
    public string Token { get; }

    public VerifyUserAuthenticationTokenCommand(string email, string token)
    {
        Email = email;
        Token = token;
    }
}
