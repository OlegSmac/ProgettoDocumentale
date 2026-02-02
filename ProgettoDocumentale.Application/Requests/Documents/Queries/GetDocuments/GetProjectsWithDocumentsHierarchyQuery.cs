using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments
{
    public class GetProjectsWithDocumentsHierarchyQuery : IRequest<List<ProjectsReportsTreeDTO>>
    { }

    public class GetProjectsWithDocumentsHierarchyQueryHandler : IRequestHandler<GetProjectsWithDocumentsHierarchyQuery, List<ProjectsReportsTreeDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetProjectsWithDocumentsHierarchyQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectsReportsTreeDTO>> Handle(GetProjectsWithDocumentsHierarchyQuery request, CancellationToken cancellationToken)
        {
            var rows = await (
                from d in _context.Documents
                join p in _context.Projects on d.ProjectId equals p.Id
                join dt in _context.DocumentTypes on d.TypeId equals dt.Id
                where dt.Code == "PRJ_ANALISI" ||
                    dt.Code == "PRJ_TRANSIZIONE" ||
                    dt.Code == "PRJ_PRODUZIONE" ||
                    dt.Code == "PRJ_TEST" ||
                    dt.Code == "PRJ_MONITORAGGIO"
                select new
                {
                    d.Id,
                    d.Name,
                    Project = p.Name,
                    Type = dt.Name
                }
            ).ToListAsync(cancellationToken);

            var result = rows
                .GroupBy(r => r.Project)
                .Select(projectGroup => new ProjectsReportsTreeDTO
                {
                    Project = projectGroup.Key,
                    ProjectTypes = projectGroup
                        .GroupBy(r => r.Type)
                        .Select(typeGroup => new ProjectTypesTreeDTO
                        {
                            Name = typeGroup.Key,
                            Count = typeGroup.Count(),
                            Reports = typeGroup.Select(report => new Common.DTOs.IdNameDTO
                            {
                                Id = report.Id,
                                Name = report.Name
                            })
                            .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }
    }
}
