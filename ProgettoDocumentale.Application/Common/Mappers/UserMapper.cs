using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using ProgettoDocumentale.Application.Requests.Projects.DTOs;
using ProgettoDocumentale.Application.Requests.Users.DTOs;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Common.Mappers
{
    public static class UserMapper
    {
        public static UserDTO UserToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                InstitutionId = user.InstitutionId,
                InstitutionName = user.Institution.Name,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                RolesIds = user.UserRoles
                    .Where(ur => ur.Role != null)
                    .Select(ur => ur.Role.Id)
                    .ToList(),
                Roles = user.UserRoles
                    .Where(ur => ur.Role != null)
                    .Select(ur => ur.Role.Name)
                    .ToList()
            };
        }

        public static User CreateUserRequestDataToUser(CreateUserRequestData req)
        {
            return new User
            {
                InstitutionId = req.InstitutionId,
                UserName = req.UserName,
                Email = req.Email,
                IsEnabled = req.IsEnabled,
                Name = req.Name,
                Surname = req.Surname,
                Patronymic = req.Patronymic
            };
        }

        public static Expression<Func<User, UserDTO>> ToDtoExpr() => user => new UserDTO
        {
            Id = user.Id,
            InstitutionId = user.InstitutionId,
            InstitutionName = user.Institution.Name,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            IsEnabled = user.IsEnabled,
            Name = user.Name,
            Surname = user.Surname,
            Patronymic = user.Patronymic,
            RolesIds = user.UserRoles
                    .Where(ur => ur.Role != null)
                    .Select(ur => ur.Role.Id)
                    .ToList(),
            Roles = user.UserRoles
                    .Where(ur => ur.Role != null)
                    .Select(ur => ur.Role.Name)
                    .ToList()
        };

    }
}
