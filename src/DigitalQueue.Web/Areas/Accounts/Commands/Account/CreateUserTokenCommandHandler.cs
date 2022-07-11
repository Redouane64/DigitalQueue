using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Services;
using DigitalQueue.Web.Areas.Notifications.Models;
using DigitalQueue.Web.Areas.Notifications.Services;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Account;

public class CreateUserTokenCommand : IRequest
{
    public ClaimsPrincipal User { get; }
    public string TokenPurpose { get; }
    public string Transport { get; }
    public string? DeviceToken { get; }

    public CreateUserTokenCommand(ClaimsPrincipal user, string tokenPurpose, string transport, string? deviceToken = null)
    {
        User = user;
        TokenPurpose = tokenPurpose;
        Transport = transport;
        DeviceToken = deviceToken;
    }
}

public class CreateUserTokenCommandHandler : IRequestHandler<CreateUserTokenCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly MailService _mailService;
    private readonly FirebaseService _firebaseService;

    public CreateUserTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IAuthenticationService authenticationService, 
        MailService mailService, 
        FirebaseService firebaseService)
    {
        _userManager = userManager;
        _authenticationService = authenticationService;
        _mailService = mailService;
        _firebaseService = firebaseService;
    }
    
    public async Task<Unit> Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _authenticationService.CreateUserToken(request.User, request.TokenPurpose);
        var user = await _userManager.GetUserAsync(request.User);

        switch (request.Transport)
        {
            case "Email":
                var notification = await MailNotification.CreateAuthenticationTokenNotification(user.Email, token!);
                await _mailService.Send(notification);
                
                break;
            
            case "PushNotification":
                await _firebaseService.Send(
                    new FirebaseNotification(
                        new[] { request.DeviceToken }!, 
                        "Digital Queue token", 
                        $"Your Digital Queue token is: {token}"
                    )
                );
                
                break;
            
        }
        
        return Unit.Value;
    }
}
