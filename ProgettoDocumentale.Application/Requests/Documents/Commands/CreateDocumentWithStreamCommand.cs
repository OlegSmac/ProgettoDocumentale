using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    public class CreateDocumentWithStreamCommand : IRequest<Unit>
    {
        public CreateDocumentWithoutFileRequestData DocumentRequest { get; set; }
        public Stream FileStream { get; set; }
        public string FileName { get; set; }              
    }

    public class CreateDocumentWithStreamCommandHandler : IRequestHandler<CreateDocumentWithStreamCommand, Unit>
    {
        private readonly IProgettoDocContext _context;
        private readonly IConfiguration _configuration;        

        public CreateDocumentWithStreamCommandHandler(
            IProgettoDocContext context,
            IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));            
        }

        private string SaveStreamToFile(Stream stream, string originalFileName)
        {
            var configRoot = _configuration.UploadsRootPhysical;
            if (string.IsNullOrWhiteSpace(configRoot)) throw new Exception("Uploads root is not configured.");

            Directory.CreateDirectory(configRoot);

            var ext = Path.GetExtension(originalFileName ?? string.Empty);
            var storedFileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(configRoot, storedFileName);
            
            if (stream.CanSeek) stream.Position = 0;
            using (var outFs = File.Create(fullPath))
            {
                stream.CopyTo(outFs);
            }

            return storedFileName;
        }

        public async Task<Unit> Handle(CreateDocumentWithStreamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request?.DocumentRequest ?? throw new ArgumentNullException(nameof(request));

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == req.InstitutionId, cancellationToken);
                if (institution == null) throw new Exception($"Institution with id={req.InstitutionId} not found");

                var type = await _context.DocumentTypes.FirstOrDefaultAsync(dt => dt.Id == req.TypeId, cancellationToken);
                if (type == null) throw new Exception($"Type with id={req.TypeId} not found");

                if (req.ProjectId.HasValue)
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.ProjectId, cancellationToken);
                    if (project == null) throw new Exception($"Project with id={req.ProjectId} not found");
                }

                if (request.FileStream == null) throw new Exception("File stream is null.");
                req.SavedPath = SaveStreamToFile(request.FileStream, request.FileName);

                var document = new Document
                {
                    InstitutionId = req.InstitutionId,
                    UserId = 1, //1 is admin id
                    TypeId = req.TypeId,
                    ProjectId = req.ProjectId,
                    Name = req.Name,
                    SavedPath = req.SavedPath,
                    UploadDate = DateTime.Now,
                    AdditionalInfo = req.AdditionalInfo,
                    GroupingDate = req.GroupingDate == null ? DateTime.Now : req.GroupingDate
                };
                
                _context.Documents.Add(document);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Unit.Value;
            }
            catch (Exception e)
            {                
                throw new Exception(e.Message);
            }
        }
    }
}
