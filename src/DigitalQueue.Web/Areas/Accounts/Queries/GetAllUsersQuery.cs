using DigitalQueue.Web.Areas.Accounts.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public partial class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
{
}
