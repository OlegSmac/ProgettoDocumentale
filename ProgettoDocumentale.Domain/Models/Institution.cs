using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class Institution : AuditableEntity
    {
        public string InstCode { get; set; }
        public string Name { get; set; }
        public string AdditionalInfo { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

    }
}
