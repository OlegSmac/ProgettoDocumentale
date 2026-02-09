using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;
using ProgettoDocumentale.Application.Requests.Documents.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    public class UpdateDocumentCommand : IRequest<Unit>
    {
        public UpdateDocumentRequestData DocumentRequest;
    }

    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, Unit>
    {
        private readonly IProgettoDocContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateDocumentCommandHandler(IProgettoDocContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.DocumentRequest;

                var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == req.Id, cancellationToken);
                if (document == null) throw new Exception($"Document with id={req.Id} not found");

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId, cancellationToken);
                if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");                

                var type = await _context.DocumentTypes.FirstOrDefaultAsync(dt => dt.Id == req.TypeId, cancellationToken);
                if (type == null) throw new Exception($"Type with id={req.InstitutionId} not found");

                if (req.ProjectId.HasValue)
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.ProjectId, cancellationToken);
                    if (project == null) throw new Exception($"Project with id={req.ProjectId} not found");
                }

                document.InstitutionId = req.InstitutionId;
                document.TypeId = req.TypeId;
                document.ProjectId = req.ProjectId;
                document.UserId = _currentUserService.UserId;
                document.Name = req.Name;                          
                document.AdditionalInfo = req.AdditionalInfo;
                document.GroupingDate = req.GroupingDate;

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
