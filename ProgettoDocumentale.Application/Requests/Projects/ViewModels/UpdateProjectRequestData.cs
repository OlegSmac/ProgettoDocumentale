using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Projects.ViewModels
{
    public class UpdateProjectRequestData
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; } = -1;
        public string Username { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTill { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
