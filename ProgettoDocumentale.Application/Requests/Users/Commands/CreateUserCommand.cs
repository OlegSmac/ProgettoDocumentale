using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Abstractions;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Domain.Models;
using ProgettoDocumentale.Application.Services;
using ProgettoDocumentale.Application.Requests.Users.DTOs;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;
using ProgettoDocumentale.Application.Common.Mappers;

namespace ProgettoDocumentale.Application.Requests.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDTO>
    {
        public CreateUserRequestData UserRequest { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IProgettoDocContext _context;        

        public CreateUserCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.UserRequest;

                var institutionExists = await _context.Institutions.AnyAsync(i => i.Id == req.InstitutionId, cancellationToken);
                if (!institutionExists) throw new Exception($"Institution with id={req.InstitutionId} not found");

                var usernameTaken = await _context.Users.AnyAsync(u => u.UserName == req.UserName, cancellationToken);
                if (usernameTaken) throw new Exception($"UserName '{req.UserName}' already exists");

                User user = UserMapper.CreateUserRequestDataToUser(req);
                user.PasswordHash = PasswordEncryptionService.HashPassword(req.Password);

                _context.Users.Add(user);

                if (req.Roles != null && req.Roles.Count > 0)
                {
                    var roles = await _context.Roles
                        .Where(r => req.Roles.Contains(r.Name))
                        .ToListAsync(cancellationToken);

                    var missing = req.Roles.Except(roles.Select(r => r.Name)).ToList();
                    if (missing.Count > 0) throw new Exception("Unknown roles: " + string.Join(", ", missing));

                    foreach (var role in roles)
                    {
                        user.UserRoles.Add(new UserToRole
                        {
                            User = user,                            
                            Role = role,
                            RoleId = role.Id
                        });
                    }
                }
                
                await _context.SaveChangesAsync(cancellationToken);

                return UserMapper.UserToUserDTO(user);
            } 
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
