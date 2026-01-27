using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Projects.Commands;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    public class RemoveDocumentByIdCommand : IRequest<string>
    {
        public int Id { get; set; }
    }

    public class RemoveDocumentByIdCommandHandler : IRequestHandler<RemoveDocumentByIdCommand, string>
    {
        private readonly IProgettoDocContext _context;

        public RemoveDocumentByIdCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(RemoveDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(i => i.Id == request.Id);
                if (document == null) return $"Document with id = {request.Id} doesn't exist";

                _context.Documents.Remove(document);
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
