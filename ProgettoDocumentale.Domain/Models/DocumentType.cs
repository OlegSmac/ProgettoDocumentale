using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class DocumentType : AuditableEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TypeDscr { get; set; }
        public bool IsMarco { get; set; }
        public bool IsDateGrouped { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<DocumentTypeHierarchy> MacroTypes { get; set; } = new List<DocumentTypeHierarchy>();
        public ICollection<DocumentTypeHierarchy> MicroTypes { get; set; } = new List<DocumentTypeHierarchy>();
    }
}
