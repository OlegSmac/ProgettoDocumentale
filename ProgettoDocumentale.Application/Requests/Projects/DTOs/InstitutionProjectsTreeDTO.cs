using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Projects.DTOs
{
    public class InstitutionProjectsTreeDTO
    {
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public List<int> Years { get; set; }
    }
}
