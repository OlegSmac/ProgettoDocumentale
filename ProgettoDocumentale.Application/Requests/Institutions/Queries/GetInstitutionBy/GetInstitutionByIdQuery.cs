using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Institutions.DTOs;
using ProgettoDocumentale.Application.Conmmon.Mappers;

namespace ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutionBy
{
    public class GetInstitutionByIdQuery : IRequest<InstitutionDTO>
    {
        public int Id { get; set; }
    }

    public class GetInstitutionByIdQueryHandler : IRequestHandler<GetInstitutionByIdQuery, InstitutionDTO>
    {
        private readonly IProgettoDocContext _context;

        public GetInstitutionByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<InstitutionDTO> Handle(GetInstitutionByIdQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                var institution = await _context.Institutions
                    .AsNoTracking()
                    .Select(InstitutionMapper.ToDtoExpr())
                    .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
                
                if (institution == null) throw new Exception($"Institution with id={request.Id} not found");

                return institution;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
