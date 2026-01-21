using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Extensions;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.TableParameters;
using ProgettoDocumentale.Application.Conmmon.Mappers;
using ProgettoDocumentale.Application.Requests.Institutions.DTOs;

namespace ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions
{
    public class GetPagedInstitutionsQuery : IRequest<IEnumerable<InstitutionDTO>>
    {
        public DataTableParameters Parameters { get; set; }
    }

    public class GetPagedInstitutionsQueryHandler : IRequestHandler<GetPagedInstitutionsQuery, IEnumerable<InstitutionDTO>>
    {
        private readonly IProgettoDocContext _context;

        public GetPagedInstitutionsQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InstitutionDTO>> Handle(GetPagedInstitutionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Institutions
                    .Search(request.Parameters)
                    .OrderBy(request.Parameters)
                    .Page(request.Parameters);

                var institutions = await query.ToListAsync(cancellationToken);

                return institutions.Select(InstitutionMapper.InstitutionToInstitutionDTO).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
