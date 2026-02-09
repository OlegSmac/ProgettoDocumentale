using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.DTOs;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Application.Requests.DocumentTypes.Queries
{
    public class GetDocumentMicroTypesByMacroIdAndNameQuery : IRequest<IEnumerable<IdNameDTO>>
    {
        public int MacroId { get; set; }
    }

    public class GetDocumentMicroTypesByMacroIdAndNameQueryHandler : IRequestHandler<GetDocumentMicroTypesByMacroIdAndNameQuery, IEnumerable<IdNameDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetDocumentMicroTypesByMacroIdAndNameQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdNameDTO>> Handle(GetDocumentMicroTypesByMacroIdAndNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.DocumentTypeHierarchies
                    .AsNoTracking()
                    .Where(h => h.MacroId == request.MacroId)
                    .Select(h => new IdNameDTO { Id = h.MicroId, Name = h.Micro.Name })
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
