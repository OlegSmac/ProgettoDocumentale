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
using ProgettoDocumentale.Application.Requests.Documents.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments
{
    public class GetPagedDocumentsQuery : IRequest<IEnumerable<DocumentDTO>>
    {
        public DataTableParameters Parameters { get; set; }
        public int? InstitutionId { get; set; }
        public int? Year { get; set; }
        public int? MacroId { get; set; }
    }

    public class GetPagedDocumentsQueryHandler : IRequestHandler<GetPagedDocumentsQuery, IEnumerable<DocumentDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetPagedDocumentsQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentDTO>> Handle(GetPagedDocumentsQuery request, CancellationToken cancellationToken)
        {
            try {
                var baseQuery =
                    from d in _context.Documents.AsNoTracking()
                    join dt in _context.DocumentTypes.AsNoTracking() on d.TypeId equals dt.Id
                    join dth in _context.DocumentTypeHierarchies.AsNoTracking() on dt.Id equals dth.MicroId into hJoin
                    from dth in hJoin.DefaultIfEmpty()
                    select new { d, dt, dth };

                if (request.InstitutionId.HasValue) baseQuery = baseQuery.Where(x => x.d.InstitutionId == request.InstitutionId.Value);
                if (request.Year.HasValue) baseQuery = baseQuery.Where(x => x.d.UploadDate.Year == request.Year.Value);
                if (request.MacroId.HasValue)
                {
                    var macroId = request.MacroId.Value;

                    baseQuery = baseQuery.Where(x =>
                        //SLA is macro type, if no SLA use macro type of micro
                        (x.dt.IsMarco && x.dt.Id == macroId)
                        || (!x.dt.IsMarco && x.dth != null && x.dth.MacroId == macroId)
                    );
                }

                var dtoQuery = baseQuery
                    .AsNoTracking()
                    .Select(x => x.d)
                    .Include(d => d.User)
                    .Include(d => d.Type)
                    .Include(d => d.Institution)
                    .Include(d => d.Project)
                    .Select(DocumentMapper.ToDtoExpr())
                    .SearchCombined(request.Parameters)
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
