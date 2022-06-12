using DigitalQueue.Web.Areas.Common.Dtos;
using DigitalQueue.Web.Areas.Teachers.Dtos;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Teachers.Queries;

public partial class SearchTeacherQuery
{
    public class SearchTeacherQueryHandler : IRequestHandler<SearchTeacherQuery, SearchResult<TeacherDto>>
    {
        private readonly UserManager<User> _userManager;

        public SearchTeacherQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<SearchResult<TeacherDto>> Handle(SearchTeacherQuery request, CancellationToken cancellationToken)
        {
            var teachers = await _userManager.Users
                .AsNoTracking()
                .Where(
                    u => EF.Functions.Like(u.Name, $"%{request.Query}%"))
                .Select(u => new TeacherDto(u.Name, u.Id))
                .ToArrayAsync(cancellationToken);

            return new SearchResult<TeacherDto>(teachers);
        }
    }
}
