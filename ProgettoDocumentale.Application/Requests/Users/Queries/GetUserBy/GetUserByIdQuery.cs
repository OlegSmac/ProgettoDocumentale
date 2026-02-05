using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Services;
using ProgettoDocumentale.Application.Requests.Users.DTOs;
using ProgettoDocumentale.Application.Common.Mappers;

namespace ProgettoDocumentale.Application.Requests.Users.Queries.GetUserBy
{
    public class GetUserByIdQuery : IRequest<UserDTO>
    {
        public int Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IProgettoDocContext _context;

        public GetUserByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Institution)
                    .Include(u => u.UserRoles)
                    .Include(u => u.UserRoles.Select(ur => ur.Role))
                    .Select(UserMapper.ToDtoExpr())
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
