using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.PermissionsList;

public class PermissionsListViewComponent : ViewComponent
{
    private readonly IMediator _mediator;
    public PermissionsListViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string userId)
    {
        return View(new UserPermissionsDto(await _mediator.Send(new GetUserPermissionsQuery(userId))));
    }
}
