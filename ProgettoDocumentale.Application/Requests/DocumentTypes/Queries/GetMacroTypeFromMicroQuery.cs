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

namespace ProgettoDocumentale.Application.Requests.DocumentTypes.Queries
{
    public class GetMacroTypeFromMicroQuery : IRequest<DocumentTypeDTO>
    {
        public int Id { get; set; }
    }

    public class GetMacroTypeFromMicroQueryHandler : IRequestHandler<GetMacroTypeFromMicroQuery, DocumentTypeDTO>
    {
        private readonly IProgettoDocContext _context;

        public GetMacroTypeFromMicroQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<DocumentTypeDTO> Handle(GetMacroTypeFromMicroQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var documentTypeMicro = await _context.DocumentTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dt => dt.Id == request.Id);

                if (documentTypeMicro == null) throw new Exception($"Document type with id={request.Id} not found");

                var documentTypeMacro = await _context.DocumentTypeHierarchies
                    .Where(h => h.MicroId == request.Id)
                    .Select(h => h.Macro)
                    .FirstOrDefaultAsync();

                return new DocumentTypeDTO
                {
                    Id = documentTypeMacro.Id,
                    Code = documentTypeMacro.Code,
                    Name = documentTypeMacro.Name,
                    IsMacro = documentTypeMacro.IsMarco
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
