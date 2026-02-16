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
using ProgettoDocumentale.Application.Requests.Projects.DTOs;

namespace ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjects
{
    public class GetAllProjectsQuery : IRequest<List<ProjectDTO>>
    { }

    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDTO>>
    {
        private readonly IProgettoDocContext _context;
        public GetAllProjectsQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }
        public async Task<List<ProjectDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .AsNoTracking()
                    .Select(ProjectMapper.ToDtoExpr())
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
