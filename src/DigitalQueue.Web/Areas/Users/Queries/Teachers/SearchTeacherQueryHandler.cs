using DigitalQueue.Web.Areas.Common.Models;
using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Users.Queries.Teachers;

public class SearchTeacherQuery : IRequest<SearchResult<TeacherAccount>>
{
    public string Query { get; }

    public SearchTeacherQuery(string query)
    {
        Query = query;
    }
}

public class SearchTeacherQueryHandler : IRequestHandler<SearchTeacherQuery, SearchResult<TeacherAccount>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public SearchTeacherQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<SearchResult<TeacherAccount>> Handle(SearchTeacherQuery request, CancellationToken cancellationToken)
    {
        var teachers = await _userManager.Users
            .AsNoTracking()
            .Where(
                u => EF.Functions.Like(u.Name, $"%{request.Query}%"))
            .Select(u => new TeacherAccount(u.Name, u.Id))
            .ToArrayAsync(cancellationToken);

        return new SearchResult<TeacherAccount>(teachers);
    }
}
