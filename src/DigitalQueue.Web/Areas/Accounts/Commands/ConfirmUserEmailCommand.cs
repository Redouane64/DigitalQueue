using System.Text.RegularExpressions;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class ConfirmUserEmailCommand : IRequest<bool>
{
    public string? UserId { get; }
    public string? Email { get; }
    public string Token { get; }

    public ConfirmUserEmailCommand(string token, string? userId = null, string? email = null)
    {
        UserId = userId;
        Email = email;
        Token = token;
    }
    
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmUserEmailCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(
            UserManager<User> userManager,
            DigitalQueueContext context,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                User user = null;
                if (request.UserId is not null)
                {
                    user = await this._userManager.FindByIdAsync(request.UserId);
                }
                else if (request.Email is not null && user is null)
                {
                    user = await this._userManager.FindByEmailAsync(request.Email);
                }

                if (user is null)
                {
                    _logger.LogWarning("Unable to find user with the provided email confirmation code");
                    return false;
                }

                string provider;
                if (Regex.Match(request.Token, "\\d{6}", RegexOptions.Compiled).Success)
                {
                    provider = SixDigitsTokenProvider.ProviderName;
                }
                else
                {
                    provider = StringTokenProvider.ProviderName;
                }
                
                var success = await this._userManager.VerifyUserTokenAsync(
                    user, 
                    provider, 
                    UserManager<User>.ConfirmEmailTokenPurpose, 
                    request.Token);

                if (!success)
                {
                    _logger.LogError("Unable to verify e-mail address");
                    return false;
                }

                user.EmailConfirmed = true;
                _context.Entry(user).Property(e => e.EmailConfirmed).IsModified = true;
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("user {Email} confirmed successfully", user.Email);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                
                _logger.LogError(e, "Unable to verify e-mail address");
                return false;
            }

            return true;
        }
    }
}
