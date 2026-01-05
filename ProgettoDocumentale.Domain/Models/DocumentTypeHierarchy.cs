using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class DocumentTypeHierarchy : AuditableEntity
    {
        public int MacroId { get; set; }
        public DocumentType Macro { get; set; }

        public int MicroId { get; set; }
        public DocumentType Micro { get; set; }
    }
}
