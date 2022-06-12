using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdateNameCommand : IRequest<bool>
{
    public string? Name { get; }

    public UpdateNameCommand(string name)
    {
        Name = name;
    }
}
