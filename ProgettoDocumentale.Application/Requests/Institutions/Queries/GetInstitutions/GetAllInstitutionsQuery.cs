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
using ProgettoDocumentale.Application.Conmmon.Mappers;
using ProgettoDocumentale.Application.Requests.Institutions.DTOs;

namespace ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions
{
    public class GetAllInstitutionsQuery : IRequest<List<InstitutionDTO>>
    { }

    public class GetAllInstitutionsQueryHandler : IRequestHandler<GetAllInstitutionsQuery, List<InstitutionDTO>>
    {
        private readonly IProgettoDocContext _context;
        public GetAllInstitutionsQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }
        public async Task<List<InstitutionDTO>> Handle(GetAllInstitutionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Institutions
                    .AsNoTracking()
                    .Select(InstitutionMapper.ToDtoExpr())
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }                     
        }
    }
}
