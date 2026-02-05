using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Conmmon.Mappers;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;

namespace ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutionBy
{
    public class GetUpdateInstitutionByIdQuery : IRequest<UpdateInstitutionRequestData>
    {
        public int Id { get; set; }
    }

    public class GetUpdateInstitutionByIdQueryHandler : IRequestHandler<GetUpdateInstitutionByIdQuery, UpdateInstitutionRequestData>
    {
        private readonly IProgettoDocContext _context;

        public GetUpdateInstitutionByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UpdateInstitutionRequestData> Handle(GetUpdateInstitutionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var institution = await _context.Institutions
                    .Select(i => new UpdateInstitutionRequestData
                    {
                        Id = i.Id,
                        InstCode = i.InstCode,
                        Name = i.Name,
                        AdditionalInfo = i.AdditionalInfo
                    })
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
