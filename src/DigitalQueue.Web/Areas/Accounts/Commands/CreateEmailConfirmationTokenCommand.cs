using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreateEmailConfirmationTokenCommand : IRequest<bool>
{
    public enum ConfirmationMethod
    {
        Url,
        Code
    }

    public string UserId { get; }
    public ConfirmationMethod Method { get; }

    public CreateEmailConfirmationTokenCommand(string userId, ConfirmationMethod method)
    {
        UserId = userId;
        Method = method;
    }
    
    public class CreateEmailConfirmationTokenCommandHandler : IRequestHandler<CreateEmailConfirmationTokenCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly NotificationService _notificationService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateEmailConfirmationTokenCommandHandler> _logger;

        public CreateEmailConfirmationTokenCommandHandler(
            UserManager<User> userManager,
            NotificationService notificationService,
            LinkGenerator linkGenerator, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<CreateEmailConfirmationTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        public async Task<bool> Handle(CreateEmailConfirmationTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await this._userManager.FindByIdAsync(request.UserId);

                switch (request.Method)
                {
                    case ConfirmationMethod.Code:
                        var code = await _userManager.GenerateUserTokenAsync(
                            user, 
                            SixDigitsTokenProvider.ProviderName, 
                            UserManager<User>.ConfirmEmailTokenPurpose
                        );
                        
                        await this._notificationService.Send(
                            new Notification<EmailConfirmationToken>(
                                new(
                                    user.Email, 
                                    ConfirmationMethod.Code, 
                                    code)
                                )
                            );
                        
                        break;
                    
                    case ConfirmationMethod.Url:
                        var token = await _userManager.GenerateUserTokenAsync(
                            user, 
                            StringTokenProvider.ProviderName,
                            UserManager<User>.ConfirmEmailTokenPurpose);

                        var confirmationLink =
                            this._linkGenerator.GetUriByPage(_httpContextAccessor.HttpContext, 
                                "/ConfirmEmail", 
                                null, 
                                new { token, email = user.Email, area = "Accounts" });

                        await this._notificationService.Send(
                            new Notification<EmailConfirmationToken>(
                                new (
                                user.Email, 
                                ConfirmationMethod.Url, 
                                confirmationLink)
                            )
                        );
                        
                        break;
                }


                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send email confirmation token");
                return false;
            }
        }
    }
}

