using System;
using System.Data.Entity;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Documents.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    //public class CreateDocumentCommand : IRequest<Unit>
    //{
    //    public CreateDocumentRequestData DocumentRequest { get; set; }      
    //}

    //public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Unit>
    //{
    //    private readonly IProgettoDocContext _context;
    //    private readonly ICurrentUserService _currentUserService;

    //    public CreateDocumentCommandHandler(IProgettoDocContext context, ICurrentUserService currentUserService)
    //    {
    //        _context = context;
    //        _currentUserService = currentUserService;
    //    }        

    //    public async Task<Unit> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var req = request.DocumentRequest;

    //            var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId, cancellationToken);
    //            if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");                

    //            var type = await _context.DocumentTypes.FirstOrDefaultAsync(dt => dt.Id == req.TypeId, cancellationToken);
    //            if (type == null) throw new Exception($"Type with id={req.TypeId} not found");

    //            if (req.ProjectId.HasValue)
    //            {
    //                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.ProjectId, cancellationToken);
    //                if (project == null) throw new Exception($"Project with id={req.ProjectId} not found");
    //            }                

    //            Document document = DocumentMapper.CreateDocumentRequestDataToDocument(req);
    //            document.UserId = _currentUserService.UserId;

    //            _context.Documents.Add(document);
    //            await _context.SaveChangesAsync(cancellationToken);

    //            return Unit.Value;
    //        }
    //        catch (Exception e)
    //        {
    //            throw new Exception(e.Message);
    //        }
    //    }
    //}
}
