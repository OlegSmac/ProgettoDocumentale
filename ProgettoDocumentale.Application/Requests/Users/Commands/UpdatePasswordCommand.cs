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
    public class UpdatePasswordCommand : IRequest<Unit>
    {
        public ResetPasswordData PasswordRequest { get; set; }
    }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        private readonly IProgettoDocContext _context;

        public UpdatePasswordCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == request.PasswordRequest.UserName);                

                user.PasswordHash = PasswordEncryptionService.HashPassword(request.PasswordRequest.NewPassword);             
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
