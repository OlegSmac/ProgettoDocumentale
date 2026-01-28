using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocumentBy
{
    public class GetDocumentByIdQuery : IRequest<DocumentDTO>
    {
        public int Id { get; set; }
    }

    public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDTO>
    {
        private readonly IProgettoDocContext _context;

        public GetDocumentByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<DocumentDTO> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _context.Documents
                    .Include(d => d.User)
                    .Include(d => d.Type)
                    .Include(d => d.Institution)
                    .Include(d => d.Project)
                    .Select(DocumentMapper.ToDtoExpr())
                    .FirstOrDefaultAsync(d => d.Id == request.Id);

                if (document == null) throw new Exception($"Document with id={request.Id} not found");

                return document;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
