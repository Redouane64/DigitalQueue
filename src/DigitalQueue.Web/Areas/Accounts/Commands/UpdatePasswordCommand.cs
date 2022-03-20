using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdatePasswordCommand : IRequest
{

    public UpdatePasswordCommand(string currentPassword, string newPassword, string confirmNewPassword)
    {
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
        ConfirmNewPassword = confirmNewPassword;
    }
    
    public string CurrentPassword { get; }

    public string NewPassword { get; }

    public string ConfirmNewPassword { get; }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private readonly UserManager<User> _userManager;

        public UpdatePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
