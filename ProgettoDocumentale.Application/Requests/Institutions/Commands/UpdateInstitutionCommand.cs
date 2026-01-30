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
    public class UpdateInstitutionCommand : IRequest<Unit>
    {
        public UpdateInstitutionRequestData InstitutionRequest;
    }

    public class UpdateInstitutionCommandHandler : IRequestHandler<UpdateInstitutionCommand, Unit>
    {
        private readonly IProgettoDocContext _context;

        public UpdateInstitutionCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateInstitutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.InstitutionRequest;

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.Id);
                if (institution == null) throw new Exception($"Institution with id={req.Id} not found");

                institution.InstCode = req.InstCode;
                institution.Name = req.Name;
                institution.AdditionalInfo = req.AdditionalInfo;

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
