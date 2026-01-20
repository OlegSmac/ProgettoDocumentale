using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Institutions.ViewModels
{
    public class CreateInstitutionRequestData
    {
        public string InstCode { get; set; }
        public string Name { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
