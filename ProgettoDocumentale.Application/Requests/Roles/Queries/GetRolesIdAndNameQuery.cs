using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.DTOs;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Application.Requests.Roles.Queries
{
    public class GetRolesIdAndNameQuery : IRequest<IEnumerable<IdNameDTO>>
    { }

    public class GetRolesIdAndNameQueryHandler : IRequestHandler<GetRolesIdAndNameQuery, IEnumerable<IdNameDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetRolesIdAndNameQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdNameDTO>> Handle(GetRolesIdAndNameQuery request, CancellationToken cancellation)
        {
            try
            {
                return await _context.Roles
                    .Select(r => new IdNameDTO { Id = r.Id, Name = r.Name })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
