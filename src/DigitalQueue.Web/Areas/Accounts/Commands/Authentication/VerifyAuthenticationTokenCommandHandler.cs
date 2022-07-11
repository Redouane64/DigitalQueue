using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Commands.Sessions;
using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Common;
using DigitalQueue.Web.Data.Users;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Authentication;


public class VerifyAuthenticationTokenCommand : IRequest<TokenResult?>
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string? DeviceToken { get; set; }
    
    public VerifyAuthenticationTokenCommand(string email, string token)
    {
        Email = email;
        Token = token;
    }
}

public class VerifyAuthenticationTokenCommandHandler : IRequestHandler<VerifyAuthenticationTokenCommand, TokenResult?>
{
    private readonly DigitalQueueUserManager _userManager;
    private readonly DigitalQueueContext _context;
    private readonly JwtTokenService _jwtTokenService;
    private readonly IMediator _mediator;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsPrincipalFactory;
    private readonly ILogger<VerifyAuthenticationTokenCommandHandler> _logger;

    public VerifyAuthenticationTokenCommandHandler(
        DigitalQueueUserManager userManager,
        DigitalQueueContext context,
        JwtTokenService jwtTokenService,
        IMediator mediator,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsPrincipalFactory,
        ILogger<VerifyAuthenticationTokenCommandHandler> logger)
    {
        _userManager = userManager;
        _context = context;
        _jwtTokenService = jwtTokenService;
        _mediator = mediator;
        _claimsPrincipalFactory = claimsPrincipalFactory;
        _logger = logger;
    }

    public async Task<TokenResult?> Handle(VerifyAuthenticationTokenCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogWarning("User {Email} does not exist", request.Email);
            return null;
        }

        var validToken = await _userManager.VerifyUserTokenAsync(user, UserTokenProvider.ProviderName,
            UserTokenProvider.AuthenticationPurposeName, request.Token);

        if (!validToken)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogWarning("User token is not valid");
            return null;
        }

        var sessionClaim = new Claim(ClaimTypesDefaults.Session, Guid.NewGuid().ToString());
        var userClaims = await _userManager.GetClaimsAsync(user);
        var claims = userClaims.Union(new[] { sessionClaim });
        var tokens = await _jwtTokenService.GenerateToken(claims, user);

        var claimsPrinciple = await _claimsPrincipalFactory.CreateAsync(user);
        await _mediator.Send(new CreateSessionCommand(claimsPrinciple) {
            SecurityStamp = sessionClaim.Value,
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            DeviceToken = request.DeviceToken!
        }, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new TokenResult(tokens.AccessToken, tokens.RefreshToken);
    }
}

