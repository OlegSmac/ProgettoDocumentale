using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.DocumentTypes.DTOs;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.DocumentTypes.Queries
{
    public class GetDocumentTypeByIdQuery : IRequest<DocumentTypeDTO>
    {
        public int Id { get; set; }
    }

    public class GetDocumentTypeByIdQueryHandler : IRequestHandler<GetDocumentTypeByIdQuery, DocumentTypeDTO>
    {
        private readonly IProgettoDocContext _context;

        public GetDocumentTypeByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<DocumentTypeDTO> Handle(GetDocumentTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var documentType = await _context.DocumentTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dt => dt.Id == request.Id);

                if (documentType == null) throw new Exception($"Document type with id={request.Id} not found");

                return new DocumentTypeDTO
                {
                    Id = documentType.Id,
                    Code = documentType.Code,
                    Name = documentType.Name,
                    IsMacro = documentType.IsMarco
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
