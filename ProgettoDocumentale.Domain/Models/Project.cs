using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class Project : AuditableEntity
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
