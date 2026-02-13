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
        public CreateDocumentWithStreamRequestData DocumentRequest { get; set; }
    }

    public class CreateDocumentWithStreamCommandHandler : IRequestHandler<CreateDocumentWithStreamCommand, Unit>
    {
        private readonly IProgettoDocContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;

        public CreateDocumentWithStreamCommandHandler(
            IProgettoDocContext context,
            IConfiguration configuration,
            ICurrentUserService currentUserService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        private string SaveStreamToFile(Stream stream, string originalFileName)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

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

            if (stream.CanSeek) stream.Position = 0;

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
                if (type == null) throw new Exception($"Type with id={req.InstitutionId} not found");

                if (req.ProjectId.HasValue)
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == req.ProjectId, cancellationToken);
                    if (project == null) throw new Exception($"Project with id={req.ProjectId} not found");
                }

                if (req.FileStream == null) throw new Exception("File stream is null.");
                req.SavedPath = SaveStreamToFile(req.FileStream, req.FileName);

                var document = new Document
                {
                    InstitutionId = req.InstitutionId,
                    UserId = 1, //1 is admin id
                    TypeId = req.TypeId,
                    ProjectId = req.ProjectId,
                    Name = req.Name,
                    SavedPath = req.SavedPath,
                    UploadDate = req.UploadDate,
                    AdditionalInfo = req.AdditionalInfo,
                    GroupingDate = req.GroupingDate
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
