using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Application.Common.DTOs;

namespace ProgettoDocumentale.Application.Requests.Documents.DTOs
{
    public class SLAReportsTreeDTO
    {
        public int Year { get; set; }
        public List<SLAMonthsTreeDTO> Months { get; set; }
    }

    public class SLAMonthsTreeDTO
    {
        public string Month { get; set; }

        public int Count { get; set; }

        public List<IdNameDTO> Reports { get; set; }
    }
    
}
