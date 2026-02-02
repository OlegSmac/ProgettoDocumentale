using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Application.Common.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.DTOs
{
    public class ServiceReportsTreeDTO
    {
        public int Year { get; set; }
        public List<ServiceMonthsTreeDTO> Months { get; set; }
    }

    public class ServiceMonthsTreeDTO { 
        public string Month { get; set; }

        public List<ServiceTypesTreeDTO> ServiceTypes { get; set; }
    }

    public class ServiceTypesTreeDTO
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public List<IdNameDTO> Reports { get; set; }
    }
    
}
