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
                var query = _context.Documents
                    .Select(DocumentMapper.ToDtoExpr())
                    .Search(request.Parameters)
                    .OrderBy(request.Parameters)
                    .Page(request.Parameters);                

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
