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
using ProgettoDocumentale.Application.Requests.Projects.DTOs;
using ProgettoDocumentale.Application.Requests.Projects.ViewModels;

namespace ProgettoDocumentale.Application.Requests.Projects.Commands
{
    public class UpdateProjectCommand : IRequest<Unit>
    {
        public UpdateProjectRequestData ProjectRequest;
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Unit>
    {
        private readonly IProgettoDocContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProjectCommandHandler(IProgettoDocContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.ProjectRequest;

                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.Id);
                if (project == null) throw new Exception($"Project with id={req.Id} not found");

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId);
                if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");               

                project.InstitutionId = institution.Id;
                project.UserId = _currentUserService.UserId;
                project.Name = req.Name;
                project.DateFrom = req.DateFrom;
                project.DateTill = req.DateTill;
                project.AdditionalInfo = req.AdditionalInfo;
                project.IsActive = req.IsActive;

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
