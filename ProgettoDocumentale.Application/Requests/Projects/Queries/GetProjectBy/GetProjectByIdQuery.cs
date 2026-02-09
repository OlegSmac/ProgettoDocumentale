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

namespace ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjectBy
{
    public class GetProjectByIdQuery : IRequest<ProjectDTO>
    {
        public int Id { get; set; }
    }

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO>
    {
        private readonly IProgettoDocContext _context;

        public GetProjectByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<ProjectDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects
                    .AsNoTracking()
                    .Include(p => p.Institution)
                    .Include(p => p.User)
                    .Select(ProjectMapper.ToDtoExpr())
                    .FirstOrDefaultAsync(p => p.Id == request.Id);
                
                if (project == null) throw new Exception($"Project with id={request.Id} not found");

                return project;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);                
            }
        }
    }
}
