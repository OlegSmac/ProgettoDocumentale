using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class Document : AuditableEntity
    {
        public int Id { get; set; }
        
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TypeId { get; set; }
        public DocumentType Type { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string Name { get; set; }
        public string SavedPath { get; set; }
        public DateTime UploadDate { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime GroupingDate { get; set; }
    }
}
