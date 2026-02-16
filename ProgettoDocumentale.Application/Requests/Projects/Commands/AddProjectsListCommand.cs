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
using ProgettoDocumentale.Application.Requests.Projects.ViewModels;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Projects.Commands
{
    public class AddProjectsListCommand : IRequest<Unit>
    {
        public List<CreateProjectRequestData> Projects { get; set; }
    }

    public class AddProjectsListCommandHandler : IRequestHandler<AddProjectsListCommand, Unit>
    {
        private readonly IProgettoDocContext _context;

        public AddProjectsListCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddProjectsListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var req in request.Projects)
                {
                    var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId, cancellationToken);
                    if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");

                    Project project = ProjectMapper.CreateProjectRequestDataToProject(req);
                    project.UserId = 1; //1 is admin id

                    _context.Projects.Add(project);
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
