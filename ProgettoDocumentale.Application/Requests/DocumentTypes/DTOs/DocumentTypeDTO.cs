using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.DocumentTypes.DTOs
{
    public class DocumentTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsMacro { get; set; }
    }
}
