using DigitalQueue.Web.Areas.Common.Dtos;
using DigitalQueue.Web.Areas.Teachers.Dtos;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Teachers.Commands;

public class SearchTeacherCommand : IRequest<SearchResult<TeacherDto>>
{
    public string Query { get; }

    public SearchTeacherCommand(string query)
    {
        Query = query;
    }
    
    public class SearchTeacherCommandHandler : IRequestHandler<SearchTeacherCommand, SearchResult<TeacherDto>>
    {
        private readonly UserManager<User> _userManager;

        public SearchTeacherCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<SearchResult<TeacherDto>> Handle(SearchTeacherCommand request, CancellationToken cancellationToken)
        {
            var teachers = await _userManager.Users.Where(
                u => EF.Functions.Like(u.FullName, $"%{request.Query}%"))
                .Select(u => new TeacherDto(u.FullName, u.Id))
                .ToArrayAsync(cancellationToken);

            return new SearchResult<TeacherDto>(teachers);
        }
    }
}
