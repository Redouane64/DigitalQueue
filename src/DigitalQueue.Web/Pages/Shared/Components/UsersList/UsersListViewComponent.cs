using DigitalQueue.Web.Areas.Accounts.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.UsersList;

public class UsersListViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public UsersListViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? page, int? pageSize, bool fullList = true)
    {
        var users = await _mediator.Send(new GetAllUsersQuery());

        return View(new Areas.Accounts.Dtos.UsersListDto(users, page, pageSize, fullList));
    }
}
