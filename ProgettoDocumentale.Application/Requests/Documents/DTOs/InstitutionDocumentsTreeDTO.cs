using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Documents.DTOs
{
    public class InstitutionDocumentsTreeDTO
    {
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public List<YearDocumentsTreeDTO> Years { get; set; }
    }

    public class YearDocumentsTreeDTO
    {
        public int Year { get; set; }
        public List<DocumentMacroNodeDTO> Types { get; set; }
    }

    public class DocumentMacroNodeDTO
    {
        public int MacroTypeId { get; set; }
        public string MacroTypeName { get; set; }
        public int Count { get; set; }
    }

}
