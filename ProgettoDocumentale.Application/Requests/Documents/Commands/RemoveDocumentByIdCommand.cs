using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Projects.Commands;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    public class RemoveDocumentByIdCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class RemoveDocumentByIdCommandHandler : IRequestHandler<RemoveDocumentByIdCommand, Unit>
    {
        private readonly IProgettoDocContext _context;
        private readonly IConfiguration _configuration;

        public RemoveDocumentByIdCommandHandler(IProgettoDocContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void DeleteFile(string savedPath)
        {
            string configRoot = _configuration.UploadsRootPhysical;
            if (string.IsNullOrWhiteSpace(configRoot) || string.IsNullOrWhiteSpace(savedPath)) throw new Exception($"File in {savedPath} not found");
            
            var fullPath = Path.Combine(configRoot, savedPath);

            if (File.Exists(fullPath)) File.Delete(fullPath);
            else throw new Exception($"File in {fullPath} not found");
        }

        public async Task<Unit> Handle(RemoveDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(i => i.Id == request.Id);
                if (document == null) throw new Exception($"Document with id = {request.Id} doesn't exist");

                DeleteFile(document.SavedPath);

                _context.Documents.Remove(document);
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
