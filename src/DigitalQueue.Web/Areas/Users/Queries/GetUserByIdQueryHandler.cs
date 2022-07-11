using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Users.Queries;

public class GetUserByIdQuery : IRequest<UserAccount>
{
    public string Id { get; }

    public GetUserByIdQuery(string id)
    {
        Id = id;
    }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserAccount>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<UserAccount> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        return new UserAccount { Email = user.Email, Name = user.Name };
    }
}
