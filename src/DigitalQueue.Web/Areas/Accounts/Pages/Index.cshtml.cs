using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

public class Index : PageModel
{
    private readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public IEnumerable<UserDto> Users { get; set; }
    
    public async Task OnGet()
    {
        ViewData["fullList"] = true;
        Users = await this._mediator.Send(new GetRegisteredAccounts());
    }
}
