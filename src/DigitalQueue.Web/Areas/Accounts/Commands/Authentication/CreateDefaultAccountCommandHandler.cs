using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Authentication;

public class CreateDefaultAccountCommand : IRequest
{
    public string Email { get; }
    public string Password { get; }

    public CreateDefaultAccountCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class CreateDefaultAccountCommandHandler : IRequestHandler<CreateDefaultAccountCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly DigitalQueueContext _context;
    private readonly ILogger<CreateDefaultAccountCommandHandler> _logger;

    public CreateDefaultAccountCommandHandler(
        UserManager<User> userManager,
        DigitalQueueContext context,
        ILogger<CreateDefaultAccountCommandHandler> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateDefaultAccountCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = new User()
            {
                Email = request.Email,
                UserName = request.Email,
                Name = request.Email
            };

            var createUser = await _userManager.CreateAsync(
                user, request.Password
            );

            if (!createUser.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var error = createUser.Errors.Select(e => e.Description).First();
                _logger.LogWarning("Unable to create user: {error!}", error);

                return Unit.Value;
            }

            var defaultRoles = new[] { RoleDefaults.User, RoleDefaults.Administrator };
            var assignRole = await _userManager.AddToRolesAsync(user, defaultRoles);
            if (!assignRole.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var error = assignRole.Errors.Select(e => e.Description).FirstOrDefault() ?? "(null)";
                _logger.LogWarning("Unable to set user role: {error!}", error);

                return Unit.Value;
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, RoleDefaults.Administrator),
                new Claim(ClaimTypes.Role, RoleDefaults.User)
            };

            var assignClaims = await _userManager.AddClaimsAsync(user, claims);
            if (!assignClaims.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var error = assignClaims.Errors.Select(e => e.Description).First();
                _logger.LogWarning("Unable to set user claims: {error!}", error);

                return Unit.Value;
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unable to default create user");
        }

        return Unit.Value;
    }
}

