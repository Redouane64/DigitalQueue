using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Areas.Accounts.Services;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Authentication;


public class AuthenticationUserCommand : IRequest<AuthenticationResult?>
{
    public string Email { get; }
    public string Name { get; }
    public string? DeviceToken { get; }

    public AuthenticationUserCommand(string email, string name, string? deviceToken = null)
    {
        Email = email;
        Name = name;
        DeviceToken = deviceToken;
    }
}

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticationUserCommand, AuthenticationResult?>
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;

    public AuthenticateUserCommandHandler(
        IUserService userService,
        IAuthenticationService authenticationService,
        IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
    {
        _userService = userService;
        _authenticationService = authenticationService;
        _claimsPrincipalFactory = claimsPrincipalFactory;
    }

    public async Task<AuthenticationResult?> Handle(AuthenticationUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.FindUserByEmail(request.Email);
        var type = AuthenticatedUserType.Existing;
        
        if (user is null)
        {
            user = await _userService.CreateUser(
                request.Email, 
                request.Name, 
                new[] { RoleDefaults.User },
                new() { ["roles"] = RoleDefaults.User, }, 
                cancellationToken);

            if (user is not null)
            {
                type = AuthenticatedUserType.Created;
            }
        }

        if (user is null)
        {
            return null;
        }

        var claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(user);
        
        return new AuthenticationResult(claimsPrincipal) { Type = type };
    }
}

