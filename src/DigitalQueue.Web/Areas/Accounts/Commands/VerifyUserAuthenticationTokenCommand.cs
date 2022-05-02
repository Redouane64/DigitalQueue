using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class VerifyUserAuthenticationTokenCommand : IRequest<AuthenticationResultDto?>
{
    public string Email { get; }
    public string Token { get; }

    public VerifyUserAuthenticationTokenCommand(string email, string token)
    {
        Email = email;
        Token = token;
    }
    
    public class VerifyUserAuthenticationTokenCommandHandler : IRequestHandler<VerifyUserAuthenticationTokenCommand, AuthenticationResultDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ILogger<VerifyUserAuthenticationTokenCommandHandler> _logger;

        public VerifyUserAuthenticationTokenCommandHandler(
            UserManager<User> userManager, 
            DigitalQueueContext context, 
            JwtTokenService jwtTokenService, 
            ILogger<VerifyUserAuthenticationTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }
        
        public async Task<AuthenticationResultDto?> Handle(VerifyUserAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                await transaction.RollbackAsync(cancellationToken);
                
                _logger.LogWarning("User {Email} does not exist", request.Email);
                return null;
            }

            var validToken = await _userManager.VerifyUserTokenAsync(user, AuthenticationTokenProvider.ProviderName,
                "Authentication", request.Token);

            if (!validToken)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogWarning("User token is not valid");
                return null;
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var sessionClaim = new Claim(ClaimTypesDefaults.Session, Guid.NewGuid().ToString());
            var claims = userClaims.Union(new [] { sessionClaim });
            var tokenResult = await _jwtTokenService.GenerateToken(claims, user);

            await transaction.CommitAsync(cancellationToken);

            return new AuthenticationResultDto(
                sessionClaim.Value, 
                tokenResult.AccessToken, 
                tokenResult.RefreshToken,
                tokenResult.Expires);
        }
    }
}
