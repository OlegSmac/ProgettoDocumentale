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
    public class RemoveProjectByIdCommand : IRequest<string>
    {
        public int Id { get; set; }
    }

    public class RemoveProjectByIdCommandHandler : IRequestHandler<RemoveProjectByIdCommand, string>
    {
        private readonly IProgettoDocContext _context;

        public RemoveProjectByIdCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(RemoveProjectByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(i => i.Id == request.Id);
                if (project == null) return $"Project with id = {request.Id} doesn't exist";

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync(cancellationToken);

                return "removed";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
