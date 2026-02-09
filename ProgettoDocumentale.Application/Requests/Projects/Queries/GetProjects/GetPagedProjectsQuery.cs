using System;
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
using ProgettoDocumentale.Application.Requests.Projects.DTOs;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjects
{
    public class GetPagedProjectsQuery : IRequest<IEnumerable<ProjectDTO>>
    {
        public DataTableParameters Parameters { get; set; }
        public int? InstitutionId { get; set; }
        public int? Year { get; set; }
    }
    
    public class GeGetPagedProjectsQueryHandler : IRequestHandler<GetPagedProjectsQuery, IEnumerable<ProjectDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GeGetPagedProjectsQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectDTO>> Handle(GetPagedProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var baseQuery = _context.Projects.AsQueryable();

                if (request.InstitutionId.HasValue) baseQuery = baseQuery.Where(p => p.InstitutionId == request.InstitutionId.Value);
                if (request.Year.HasValue) baseQuery = baseQuery.Where(p => p.DateFrom.Year == request.Year.Value);

                var dtoQuery = baseQuery
                    .AsNoTracking()
                    .Include(p => p.Institution)
                    .Include(p => p.User)
                    .Select(ProjectMapper.ToDtoExpr())
                    .Search(request.Parameters)
                    .OrderBy(request.Parameters)
                    .Page(request.Parameters);

                return await dtoQuery.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
