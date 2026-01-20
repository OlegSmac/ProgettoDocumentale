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
    public class GetInstitutionsIdAndNameQuery : IRequest<IEnumerable<IdNameDTO>>
    { }

    public class GetInstitutionsIdAndNameQueryHandler : IRequestHandler<GetInstitutionsIdAndNameQuery, IEnumerable<IdNameDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetInstitutionsIdAndNameQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdNameDTO>> Handle(GetInstitutionsIdAndNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Institutions
                    .Select(i => new IdNameDTO { Id = i.Id, Name = i.Name })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception("GetInstitutionsIdAndNameQuery exception " + e.Message);
            }
        }
    }
}
