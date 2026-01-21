using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Data.Entity;
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
                    .Include(u => u.UserRoles)
                    .Include("UserRoles.Role")
                    .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (user == null) return null;

                return UserMapper.UserToUserDTO(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
