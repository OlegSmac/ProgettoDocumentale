using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Application.Common.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.DTOs
{
    public class ProjectsReportsTreeDTO
    {
        public string Project { get; set; }

        public List<ProjectTypesTreeDTO> ProjectTypes { get; set; }
    }

    public class ProjectTypesTreeDTO
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public List<IdNameDTO> Reports { get; set; }
    }
}
