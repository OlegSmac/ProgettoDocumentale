using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Conmmon.Mappers;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Institutions.Commands
{
    public class AddInstitutionsListCommand : IRequest<Unit>
    {
        public List<CreateInstitutionRequestData> Institutions { get; set; }
    }

    public class AddInstitutionsListCommandHandler : IRequestHandler<AddInstitutionsListCommand, Unit>
    {
        private readonly IProgettoDocContext _context;

        public AddInstitutionsListCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddInstitutionsListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var req in request.Institutions)
                {
                    var institutionInDb = await _context.Institutions.AnyAsync(i => i.InstCode == req.InstCode);
                    if (institutionInDb) throw new Exception($"Institution with {req.InstCode} already exist");

                    Institution institution = InstitutionMapper.CreateInstitutionRequestDataToInstitution(req);
                    _context.Institutions.Add(institution);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
