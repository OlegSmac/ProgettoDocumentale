using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Data.Entity;
using ProgettoDocumentale.Application.Abstractions;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Services;
using ProgettoDocumentale.Application.Requests.Users.DTOs;
using ProgettoDocumentale.Application.Common.Mappers;

namespace ProgettoDocumentale.Application.Requests.Users.Queries.GetUserBy
{
    public class GetUserByUsernameAndPasswordQuery : IRequest<UserDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByUsernameAndPasswordHandler : IRequestHandler<GetUserByUsernameAndPasswordQuery, UserDTO>
    {

        private readonly IProgettoDocContext _context;

        public GetUserByUsernameAndPasswordHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Handle(GetUserByUsernameAndPasswordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .Include("UserRoles.Role")
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

                if (user == null) return null;               
                if (!PasswordEncryptionService.VerifyPassword(request.Password, user.PasswordHash)) return null;

                return UserMapper.UserToUserDTO(user);
            }
            catch (Exception e)
            {
                throw new Exception("GetUserByUsernameAndPasswordQuery exception " + e.Message);
            }
        }
    }
}
