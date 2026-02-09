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
    public class GetDocumentsHierarchyQuery : IRequest<List<InstitutionDocumentsTreeDTO>>
    { }

    public class GetDocumentsHierarchyQueryHandler : IRequestHandler<GetDocumentsHierarchyQuery, List<InstitutionDocumentsTreeDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetDocumentsHierarchyQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<List<InstitutionDocumentsTreeDTO>> Handle(GetDocumentsHierarchyQuery request, CancellationToken cancellationToken)
        {            
            var rows = await (
                from d in _context.Documents.AsNoTracking()
                join i in _context.Institutions.AsNoTracking() on d.InstitutionId equals i.Id
                join dt in _context.DocumentTypes.AsNoTracking() on d.TypeId equals dt.Id
               
                join dth in _context.DocumentTypeHierarchies.AsNoTracking() on dt.Id equals dth.MicroId into hJoin
                from dth in hJoin.DefaultIfEmpty()

                //SLA is macro type, if no SLA use macro type of micro
                let macroId = dt.IsMarco ? dt.Id : (int?)dth.MacroId
                where macroId != null

                join mt in _context.DocumentTypes on macroId.Value equals mt.Id
                where mt.IsMarco

                group d by new
                {
                    d.InstitutionId,
                    InstitutionName = i.Name,
                    Year = d.UploadDate.Year,
                    MacroId = mt.Id,
                    MacroName = mt.Name
                }
                
                into g
                select new
                {
                    g.Key.InstitutionId,
                    g.Key.InstitutionName,
                    g.Key.Year,
                    g.Key.MacroId,
                    g.Key.MacroName,
                    Count = g.Count()
                }
            )            
            .ToListAsync(cancellationToken);
            
            var result = rows
                .GroupBy(r => new { r.InstitutionId, r.InstitutionName })
                .OrderBy(g => g.Key.InstitutionName)
                .Select(inst => new InstitutionDocumentsTreeDTO
                {
                    InstitutionId = inst.Key.InstitutionId,
                    InstitutionName = inst.Key.InstitutionName,
                    Years = inst
                        .GroupBy(r => r.Year)
                        .OrderByDescending(g => g.Key)
                        .Select(yearGroup => new YearDocumentsTreeDTO
                        {
                            Year = yearGroup.Key,
                            Types = yearGroup
                                .GroupBy(x => new { x.MacroId, x.MacroName })
                                .Select(g => new DocumentMacroNodeDTO
                                {
                                    MacroTypeId = g.Key.MacroId,
                                    MacroTypeName = g.Key.MacroName,
                                    Count = g.Sum(x => x.Count)
                                })
                                .OrderBy(t => t.MacroTypeName)
                                .ToList()
                        })
                        .Where(y => y.Types.Any())
                        .ToList()
                })
                .Where(i => i.Years.Any())                
                .ToList();

            return result;
        }
    }


}
