using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Projects.DTOs;

namespace ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjects
{
    public class GetProjectsHierarchyQuery : IRequest<List<InstitutionProjectsTreeDTO>>
    { }

    public class GetProjectsHierarchyQueryHandler : IRequestHandler<GetProjectsHierarchyQuery, List<InstitutionProjectsTreeDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetProjectsHierarchyQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<List<InstitutionProjectsTreeDTO>> Handle(GetProjectsHierarchyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .GroupBy(p => new { p.InstitutionId, p.Institution.Name })
                    .Select(g => new InstitutionProjectsTreeDTO
                    {
                        InstitutionId = g.Key.InstitutionId,
                        InstitutionName = g.Key.Name,
                        Years = g.Select(x => x.DateFrom.Year)
                                 .Distinct()
                                 .OrderBy(y => y)
                                 .ToList()
                    })
                    .OrderBy(x => x.InstitutionName)                    
                    .ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
