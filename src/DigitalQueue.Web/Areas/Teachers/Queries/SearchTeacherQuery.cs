using DigitalQueue.Web.Areas.Common.Dtos;
using DigitalQueue.Web.Areas.Teachers.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Teachers.Queries;

public partial class SearchTeacherQuery : IRequest<SearchResult<TeacherDto>>
{
    public string Query { get; }

    public SearchTeacherQuery(string query)
    {
        Query = query;
    }
}
