using DigitalQueue.Web.Areas.Accounts.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public partial class GetUserQuery : IRequest<UserDto?>
{
    public string Id { get; }

    public GetUserQuery(string id)
    {
        Id = id;
    }
}
