using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class UpdatePasswordCommand : IRequest<bool>
{

    public UpdatePasswordCommand(string email, string newPassword, string token)
    {
        Email = email;
        NewPassword = newPassword;
        Token = token;
    }

    public string Email { get; }
    public string NewPassword { get; }
    public string Token { get; }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public UpdatePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<bool> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await this._userManager.FindByEmailAsync(request.Email);
            
            // TODO: should verify old password but it is ok for now.

            if (user is null)
            {
                return false;
            }
            
            var changePasswordResult = await this._userManager.ResetPasswordAsync(
                user, 
                request.Token, 
                request.NewPassword);

            return changePasswordResult.Succeeded;
        }
    }
}
