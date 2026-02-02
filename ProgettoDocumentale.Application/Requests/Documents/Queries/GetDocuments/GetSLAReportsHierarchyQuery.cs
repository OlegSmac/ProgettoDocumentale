using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.DTOs;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments
{
    public class GetSLAReportsHierarchyQuery : IRequest<List<SLAReportsTreeDTO>>
    { }

    public class GetSLAReportsHierarchyQueryHandler : IRequestHandler<GetSLAReportsHierarchyQuery, List<SLAReportsTreeDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetSLAReportsHierarchyQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<List<SLAReportsTreeDTO>> Handle(GetSLAReportsHierarchyQuery request, CancellationToken cancellationToken)
        {
            var rows = await (
                from d in _context.Documents
                join dt in _context.DocumentTypes on d.TypeId equals dt.Id
                where dt.Code == "SLA_REPORT"
                select new
                {
                    d.Id,
                    d.Name,
                    Year = d.UploadDate.Year,
                    Month = d.UploadDate.Month,
                }
            ).ToListAsync(cancellationToken);

            var result = rows
                .GroupBy(r => r.Year)
                .OrderByDescending(g => g.Key)
                .Select(yearGroup => new SLAReportsTreeDTO
                {
                    Year = yearGroup.Key,
                    Months = yearGroup
                        .GroupBy(r => r.Month)
                        .OrderBy(g => g.Key)
                        .Select(monthGroup => new SLAMonthsTreeDTO
                        {
                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key),
                            Count = monthGroup.Count(),
                            Reports = monthGroup.Select(report => new IdNameDTO
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
