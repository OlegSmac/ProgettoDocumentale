using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Services;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;

namespace ProgettoDocumentale.Application.Requests.Users.Commands
{
    public class UpdatePasswordCommand : IRequest<string>
    {
        public ResetPasswordData PasswordRequest { get; set; }
    }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, string>
    {
        private readonly IProgettoDocContext _context;

        public UpdatePasswordCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == request.PasswordRequest.UserName);

                if (!PasswordEncryptionService.VerifyPassword(request.PasswordRequest.OldPassword, user.PasswordHash)) throw new Exception("Incorrect old password");

                user.PasswordHash = PasswordEncryptionService.HashPassword(request.PasswordRequest.NewPassword);             
                await _context.SaveChangesAsync(cancellationToken);

                return "Updated";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
