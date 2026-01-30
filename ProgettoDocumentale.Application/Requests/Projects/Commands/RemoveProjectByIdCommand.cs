using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Application.Requests.Projects.Commands
{
    public class RemoveProjectByIdCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class RemoveProjectByIdCommandHandler : IRequestHandler<RemoveProjectByIdCommand, Unit>
    {
        private readonly IProgettoDocContext _context;

        public RemoveProjectByIdCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveProjectByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(i => i.Id == request.Id);
                if (project == null) throw new Exception($"Project with id = {request.Id} doesn't exist");

                var relatedDocuments = await _context.Documents
                    .Where(d => d.ProjectId != null && d.ProjectId == project.Id)
                    .ToListAsync();

                foreach (var doc in relatedDocuments) _context.Documents.Remove(doc);

                _context.Projects.Remove(project);
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
