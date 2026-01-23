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
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Projects.Commands
{
    public class CreateProjectCommand : IRequest<ProjectDTO>
    {
        public CreateProjectRequestData ProjectRequest { get; set; }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDTO>
    {
        private readonly IProgettoDocContext _context;

        public CreateProjectCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<ProjectDTO> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.ProjectRequest;

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId, cancellationToken);
                if (institution == null) throw new Exception($"Institution with name {req.InstitutionId} not found");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == req.Username, cancellationToken);
                if (user == null) throw new Exception($"User with username '{req.Username}' not found");

                Project project = ProjectMapper.CreateProjectRequestDataToProject(req);
                project.InstitutionId = institution.Id;
                project.UserId = user.Id;

                _context.Projects.Add(project);
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
