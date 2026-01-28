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
    public class UpdateProjectCommand : IRequest<ProjectDTO>
    {
        public UpdateProjectRequestData ProjectRequest;
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDTO>
    {
        private readonly IProgettoDocContext _context;

        public UpdateProjectCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<ProjectDTO> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.ProjectRequest;

                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.Id);
                if (project == null) throw new Exception($"Project with id={req.Id} not found");

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId);
                if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == req.Username);
                if (user == null) throw new Exception($"User with username {req.Username} not found");

                project.InstitutionId = institution.Id;
                project.UserId = user.Id;
                project.Name = req.Name;
                project.DateFrom = req.DateFrom;
                project.DateTill = req.DateTill;
                project.AdditionalInfo = req.AdditionalInfo;
                project.IsActive = req.IsActive;

                await _context.SaveChangesAsync(cancellationToken);

                return ProjectMapper.ProjectToProjectDTO(project);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
