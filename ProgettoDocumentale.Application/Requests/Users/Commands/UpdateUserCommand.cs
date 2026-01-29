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
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Users.DTOs;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDTO>
    {
        public UpdateUserRequestData UserRequest { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly IProgettoDocContext _context;

        public UpdateUserCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.UserRequest;

                var user = await _context.Users
                    .Include(u => u.Institution)
                    .Include(u => u.UserRoles.Select(ur => ur.Role))
                    .FirstOrDefaultAsync(u => u.Id == req.Id);

                if (user == null) throw new Exception($"User with id={req.Id} not found");

                var institutionExists = await _context.Institutions.AnyAsync(i => i.Id == req.InstitutionId, cancellationToken);
                if (!institutionExists) throw new Exception($"Institution with id={req.InstitutionId} not found");

                var usernameTaken = await _context.Users.AnyAsync(u => u.UserName == req.UserName && u.Id != req.Id, cancellationToken);
                if (usernameTaken) throw new Exception($"UserName '{req.UserName}' already exists");

                var emailTaken = await _context.Users.AnyAsync(u => u.Email == req.Email && u.Id != req.Id, cancellationToken);
                if (emailTaken) throw new Exception($"Email '{req.Email}' already exists");

                user.InstitutionId = req.InstitutionId;
                user.UserName = req.UserName;
                user.Email = req.Email;
                user.IsEnabled = req.IsEnabled;
                user.Name = req.Name;
                user.Surname = req.Surname;
                user.Patronymic = req.Patronymic;

                var requestedRoleNames = req.Roles
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (user.UserRoles.Any())
                {
                    _context.UserToRoles.RemoveRange(user.UserRoles.ToList());
                    user.UserRoles.Clear();
                }

                if (requestedRoleNames.Count > 0)
                {
                    var roles = await _context.Roles
                        .Where(r => requestedRoleNames.Contains(r.Name))
                        .ToListAsync(cancellationToken);

                    var missing = requestedRoleNames
                        .Except(roles.Select(r => r.Name), StringComparer.OrdinalIgnoreCase)
                        .ToList();

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
