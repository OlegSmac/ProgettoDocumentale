using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Data.Entity;
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
                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == request.Id);
                if (institution == null) return null;

                return InstitutionMapper.InstitutionToInstitutionDTO(institution);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
