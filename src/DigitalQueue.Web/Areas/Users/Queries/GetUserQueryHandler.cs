using System.Security.Claims;

using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Users.Queries;

public class GetUserQuery : IRequest<UserAccount?>
{
    public ClaimsPrincipal? User { get; }

    public GetUserQuery(ClaimsPrincipal? user)
    {
        User = user;
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserAccount?>
{
    private readonly UserManager<User> _userManager;

    public GetUserQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserAccount?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(request.User);
        return new UserAccount { Email = user.Email, Name = user.Name };
    }
}

