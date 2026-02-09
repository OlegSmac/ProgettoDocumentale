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

namespace ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjects
{
    public class GetProjectsIdAndNameQuery : IRequest<IEnumerable<IdNameDTO>>
    { }

    public class GetProjectsIdAndNameQueryHandler : IRequestHandler<GetProjectsIdAndNameQuery, IEnumerable<IdNameDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetProjectsIdAndNameQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdNameDTO>> Handle(GetProjectsIdAndNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .AsNoTracking()
                    .Select(i => new IdNameDTO { Id = i.Id, Name = i.Name })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
