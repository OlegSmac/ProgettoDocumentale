using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;
using ProgettoDocumentale.Application.Requests.Documents.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    public class CreateDocumentCommand : IRequest<DocumentDTO>
    {
        public CreateDocumentRequestData DocumentRequest { get; set; }
    }

    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, DocumentDTO>
    {
        private readonly IProgettoDocContext _context;

        public CreateDocumentCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<DocumentDTO> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.DocumentRequest;

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId, cancellationToken);
                if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == req.Username, cancellationToken);
                if (user == null) throw new Exception($"User with username '{req.Username}' not found");

                var type = await _context.DocumentTypes.FirstOrDefaultAsync(dt => dt.Id == req.TypeId, cancellationToken);
                if (type == null) throw new Exception($"Type with id={req.InstitutionId} not found");

                if (req.ProjectId.HasValue)
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.ProjectId, cancellationToken);
                    if (project == null) throw new Exception($"Project with id={req.InstitutionId} not found");
                }

                Document document = DocumentMapper.CreateDocumentRequestDataToDocument(req);
                document.UserId = user.Id;

                _context.Documents.Add(document);
                await _context.SaveChangesAsync(cancellationToken);

                return DocumentMapper.DocumentToDocumentDTO(document);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
