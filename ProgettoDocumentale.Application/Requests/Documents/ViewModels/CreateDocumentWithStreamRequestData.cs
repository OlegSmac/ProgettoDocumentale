using System;
using System.IO;

namespace ProgettoDocumentale.Application.Requests.Documents.Commands
{
    public class CreateDocumentWithStreamRequestData
    {
        public int InstitutionId { get; set; } = -1;
        public int UserId { get; set; } = -1;
        public int MacroTypeId { get; set; } = -1;
        public int? MicroTypeId { get; set; }
        public int TypeId { get; set; } = -1;
        public int? ProjectId { get; set; } = -1;
        public string Name { get; set; }
        public string SavedPath { get; set; }

        
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long? FileLength { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string AdditionalInfo { get; set; }
        public DateTime GroupingDate { get; set; } = DateTime.Now;
    }
}