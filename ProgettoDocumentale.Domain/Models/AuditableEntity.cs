using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class AuditableEntity : BaseEntity
    {
        public int CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
