using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.DTOs;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Application.Requests.DocumentTypes.Queries
{
    public class GetMacroDocumentTypesIdAndNameQuery : IRequest<IEnumerable<IdNameCodeDTO>>
    { }

    public class GetMacroDocumentTypesIdAndNameQueryHandler : IRequestHandler<GetMacroDocumentTypesIdAndNameQuery, IEnumerable<IdNameCodeDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetMacroDocumentTypesIdAndNameQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdNameCodeDTO>> Handle(GetMacroDocumentTypesIdAndNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.DocumentTypes
                    .Where(dt => dt.IsMarco == true)
                    .Select(dt => new IdNameCodeDTO { 
                        Id = dt.Id, 
                        Name = dt.Name,
                        Code = dt.Code
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
