using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Extensions;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Common.TableParameters;
using ProgettoDocumentale.Application.Requests.Users.DTOs;

namespace ProgettoDocumentale.Application.Requests.Users.Queries.GetUsers
{
    public class GetPagedUsersQuery : IRequest<IEnumerable<UserDTO>>
    {
        public DataTableParameters Parameters { get; set; }
    }

    public class GetPagedUsersQueryHandler : IRequestHandler<GetPagedUsersQuery, IEnumerable<UserDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetPagedUsersQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Users
                    .Include(u => u.Institution)
                    .Include(u => u.UserRoles.Select(ur => ur.Role))
                    .Select(UserMapper.ToDtoExpr())
                    .Search(request.Parameters)
                    .OrderBy(request.Parameters)
                    .Page(request.Parameters);
                
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
