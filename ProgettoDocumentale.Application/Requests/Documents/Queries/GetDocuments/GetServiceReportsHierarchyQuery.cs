using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.DTOs;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments
{
    public class GetServiceReportsHierarchyQuery : IRequest<List<ServiceReportsTreeDTO>>
    { }

    public class GetServiceReportsHierarchyQueryHandler : IRequestHandler<GetServiceReportsHierarchyQuery, List<ServiceReportsTreeDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetServiceReportsHierarchyQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceReportsTreeDTO>> Handle(GetServiceReportsHierarchyQuery request, CancellationToken cancellationToken)
        {
            var rows = await (
                from d in _context.Documents.AsNoTracking()
                join dt in _context.DocumentTypes.AsNoTracking() on d.TypeId equals dt.Id
                where dt.Code == "SRV_NETWORK" ||
                    dt.Code == "SRV_SECURITY" ||
                    dt.Code == "SRV_CHANGE" ||
                    dt.Code == "SRV_BACKUP"
                select new
                {
                    d.Id,
                    d.Name,
                    Year = d.UploadDate.Year,
                    Month = d.UploadDate.Month,
                    Type = dt.Name
                }
            ).ToListAsync(cancellationToken);

            var result = rows
                .GroupBy(r => r.Year)
                .OrderByDescending(g => g.Key)
                .Select(yearGroup => new ServiceReportsTreeDTO
                {
                    Year = yearGroup.Key,
                    Months = yearGroup
                        .GroupBy(r => r.Month)
                        .OrderBy(g => g.Key)
                        .Select(monthGroup => new ServiceMonthsTreeDTO
                        {
                            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key),
                            ServiceTypes = monthGroup
                                .GroupBy(r => r.Type)
                                .OrderBy(g => g.Key)
                                .Select(typeGroup => new ServiceTypesTreeDTO
                                {
                                    Name = typeGroup.Key,
                                    Count = typeGroup.Count(),
                                    Reports = typeGroup.Select(report => new IdNameDTO { 
                                        Id = report.Id,
                                        Name = report.Name
                                    })
                                    .ToList()
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
