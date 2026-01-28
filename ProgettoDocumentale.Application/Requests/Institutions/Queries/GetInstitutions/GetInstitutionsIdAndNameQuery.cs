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

namespace ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions
{
    public class GetInstitutionIdAndNameQuery : IRequest<IEnumerable<IdNameDTO>>
    { }

    public class GetInstitutionsIdAndNameQueryHandler : IRequestHandler<GetInstitutionIdAndNameQuery, IEnumerable<IdNameDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetInstitutionsIdAndNameQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdNameDTO>> Handle(GetInstitutionIdAndNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Institutions
                    .Select(i => new IdNameDTO { Id = i.Id, Name = i.Name })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
