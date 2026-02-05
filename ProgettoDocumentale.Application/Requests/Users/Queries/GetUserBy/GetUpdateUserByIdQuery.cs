using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;

namespace ProgettoDocumentale.Application.Requests.Users.Queries.GetUserBy
{
    public class GetUpdateUserByIdQuery : IRequest<UpdateUserRequestData>
    {
        public int Id { get; set; }
    }

    public class GetUpdateUserByIdQueryHandler : IRequestHandler<GetUpdateUserByIdQuery, UpdateUserRequestData>
    {
        private readonly IProgettoDocContext _context;

        public GetUpdateUserByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UpdateUserRequestData> Handle(GetUpdateUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Institution)
                    .Include(u => u.UserRoles)
                    .Include(u => u.UserRoles.Select(ur => ur.Role))
                    .Select(u => new UpdateUserRequestData
                    {
                        Id = u.Id,
                        InstitutionId = u.InstitutionId,                        
                        UserName = u.UserName,                        
                        Email = u.Email,
                        IsEnabled = u.IsEnabled,
                        Name = u.Name,
                        Surname = u.Surname,
                        Patronymic = u.Patronymic,
                        Roles = u.UserRoles
                            .Where(ur => ur.Role != null)
                            .Select(ur => ur.Role.Id)
                            .ToList()                        
                    })
                    .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (user == null) throw new Exception($"User with id={request.Id} not found");

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
