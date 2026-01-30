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
using ProgettoDocumentale.Application.Requests.Institutions.DTOs;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Institutions.Commands
{
    public class CreateInstitutionCommand : IRequest<Unit>
    {
        public CreateInstitutionRequestData InstitutionRequest;
    }

    public class CreateInstitutionCommandHandler : IRequestHandler<CreateInstitutionCommand, Unit>
    {
        private readonly IProgettoDocContext _context;

        public CreateInstitutionCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.InstitutionRequest;

                var institutionInDb = await _context.Institutions.AnyAsync(i => i.InstCode == req.InstCode);
                if (institutionInDb) throw new Exception($"Institution with {req.InstCode} already exist");

                Institution institution = InstitutionMapper.CreateInstitutionRequestDataToInstitution(req);             
                _context.Institutions.Add(institution);
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
