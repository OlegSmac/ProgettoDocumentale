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
using ProgettoDocumentale.Application.DTOs.User;

namespace ProgettoDocumentale.Application.Requests.UserManagment.Queries.GetUserBy
{
    public class GetUserByUsernameAndPasswordQuery : IRequest<UserDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByUsernameAndPasswordHandler : IRequestHandler<GetUserByUsernameAndPasswordQuery, UserDTO>
    {

        private readonly IProgettoDocContext _context;
        private readonly IPasswordEncryptionService _passwordEncryptionService;

        public GetUserByUsernameAndPasswordHandler(IProgettoDocContext context, IPasswordEncryptionService passwordEncryptionService)
        {
            _context = context;
            _passwordEncryptionService = passwordEncryptionService;
        }

        public async Task<UserDTO> Handle(GetUserByUsernameAndPasswordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles) //isn't it done automatically?
                    .Include("UserRoles.Role")
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

                if (user == null)
                {
                    return null;
                }

                if (!_passwordEncryptionService.VerifyPassword(request.Password, user.PasswordHash)) return null;

                var userDto = new UserDTO
                {
                    Id = user.Id,
                    InstitutionId = user.InstitutionId,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled,
                    Name = user.Name,
                    Surname = user.Surname,
                    Patronymic = user.Patronymic,
                    Roles = user.UserRoles
                        .Where(ur => ur.Role != null)
                        .Select(ur => ur.Role.Name)
                        .ToList()
                };

                return userDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured in user query" + ex);
            }
        }
    }
}
