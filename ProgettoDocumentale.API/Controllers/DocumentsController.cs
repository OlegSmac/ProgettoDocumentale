using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ProgettoDocumentale.Application.Requests.Documents.Commands;
using ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments;

namespace ProgettoDocumentale.API.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var result = await _mediator.Send(new GetAllDocumentsQuery());

            return Ok(result);
        }

        // Expect multipart/form-data with:
        // - "metadata" (string) -> JSON array of CreateDocumentWithStreamRequestData (without streams)
        // - "files" (file[])   -> files in the same order as metadata entries (index-based matching)
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddDocumentsList([FromForm] string metadata, [FromForm] List<IFormFile> files)
        {
            if (string.IsNullOrWhiteSpace(metadata))
            {
                return BadRequest("Missing 'metadata' form field containing JSON array of documents.");
            }

            List<CreateDocumentWithStreamRequestData>? documents;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };                
                if (Request?.Form != null && Request.Form.TryGetValue("metadata", out StringValues values) && values.Count > 1)
                {                    
                    var joined = "[" + string.Join(",", values.Select(v => v.Trim())) + "]";
                    metadata = joined;
                }

                var json = metadata?.Trim() ?? string.Empty;                
                if (json.Length > 0 && json[0] == '{')
                {
                    var single = JsonSerializer.Deserialize<CreateDocumentWithStreamRequestData>(json, options);
                    documents = single != null ? new List<CreateDocumentWithStreamRequestData> { single } : new List<CreateDocumentWithStreamRequestData>();
                }
                else
                {
                    documents = JsonSerializer.Deserialize<List<CreateDocumentWithStreamRequestData>>(json, options) ?? new List<CreateDocumentWithStreamRequestData>();
                }
            }
            catch (JsonException jex)
            {
                return BadRequest($"Invalid metadata JSON: {jex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid metadata: {ex.Message}");
            }

            if (documents == null || documents.Count == 0) return BadRequest("Request metadata must contain a non-empty JSON array of documents.");        
            if (files == null || files.Count == 0) return BadRequest("No files uploaded. Provide files in the 'files' form field.");
            if (files.Count != documents.Count) return BadRequest("The number of uploaded files must match the number of documents in metadata.");

            var failures = new List<(int Index, string Error)>();

            for (int i = 0; i < documents.Count; i++)
            {
                var docMeta = documents[i];
                var file = files[i];

                if (file == null || file.Length == 0) return BadRequest($"File at index {i} is empty or missing.");                
                
                docMeta.FileStream = file.OpenReadStream();
                docMeta.FileName = file.FileName;
                docMeta.ContentType = file.ContentType;
                docMeta.FileLength = file.Length;
                
                try
                {
                    await _mediator.Send(new CreateDocumentWithStreamCommand { DocumentRequest = docMeta });
                }
                catch (Exception e)
                {
                    failures.Add((i, e.Message));                    
                    continue;
                }
                finally
                {                    
                    (docMeta.FileStream as System.IDisposable)?.Dispose();
                }
            }

            if (failures.Count > 0) return StatusCode(StatusCodes.Status207MultiStatus, failures);
            
            return Ok();
        }
    }
}
