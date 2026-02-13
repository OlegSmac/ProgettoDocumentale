using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments
{
    public class GetAllDocumentsQuery : IRequest<List<DocumentDTO>>
    { }

    public class GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery, List<DocumentDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetAllDocumentsQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<List<DocumentDTO>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Documents
                    .AsNoTracking()
                    .Include(d => d.User)
                    .Include(d => d.Type)
                    .Include(d => d.Institution)
                    .Include(d => d.Project)
                    .Select(DocumentMapper.ToDtoExpr())
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
