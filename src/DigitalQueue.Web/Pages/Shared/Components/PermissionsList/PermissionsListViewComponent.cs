using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;

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
    
    public async Task<IViewComponentResult> InvokeAsync(IEnumerable<UserCourseRolesDto> claims)
    {
        var ids = claims.Select(c => c.CourseId).ToArray();
        var courses = await this._mediator.Send(new GetCoursesByIdsQuery(ids));
        var coursesWithRoles = courses.Join(
            claims, c => c.Id,
            r => r.CourseId,
            (course, roles) => new UserCourseRolesDto(course.Id, course.Title, roles.Roles)
        );
        
        return View(new UserPermissionsDto(coursesWithRoles));
    }
}
