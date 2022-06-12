using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Common.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class SearchRoleCommand : IRequest<SearchResult<AccountRoleDto>>
{
    public SearchRoleCommand(string? query = null, IEnumerable<string>? filter = null, bool returnIds = false)
    {
        ReturnIds = returnIds;
        Query = query;
        Filter = filter;
    }

    public bool ReturnIds { get; }
    public string? Query { get; }
    public IEnumerable<string>? Filter { get; }
}
