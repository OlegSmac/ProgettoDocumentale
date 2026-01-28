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
                from d in _context.Documents
                join i in _context.Institutions on d.InstitutionId equals i.Id
                join dt in _context.DocumentTypes on d.TypeId equals dt.Id
               
                join dth in _context.DocumentTypeHierarchies on dt.Id equals dth.MicroId into hJoin
                from dth in hJoin.DefaultIfEmpty()

                //SLA is macro type, if no SLA use macro type of micro
                let macroId = dt.IsMarco ? dt.Id : (int?)dth.MacroId
                where macroId != null

                join mt in _context.DocumentTypes on macroId.Value equals mt.Id
                where mt.IsMarco
                select new
                {
                    d.InstitutionId,
                    InstitutionName = i.Name,
                    Year = d.UploadDate.Year,
                    MacroId = mt.Id,
                    MacroName = mt.Name
                }
            )
            .GroupBy(x => new
            {
                x.InstitutionId,
                x.InstitutionName,
                x.Year,
                x.MacroId,
                x.MacroName
            })
            .Select(g => new
            {
                g.Key.InstitutionId,
                g.Key.InstitutionName,
                g.Key.Year,
                g.Key.MacroId,
                g.Key.MacroName,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);
            
            var result = rows
                .GroupBy(r => new { r.InstitutionId, r.InstitutionName })
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
                                .Select(t => new DocumentMacroNodeDTO
                                {
                                    MacroTypeId = t.MacroId,
                                    MacroTypeName = t.MacroName,
                                    Count = t.Count
                                })
                                .OrderBy(t => t.MacroTypeName)
                                .ToList()
                        })
                        .Where(y => y.Types.Any())
                        .ToList()
                })
                .Where(i => i.Years.Any())
                .OrderBy(i => i.InstitutionName)
                .ToList();

            return result;
        }
    }


}
