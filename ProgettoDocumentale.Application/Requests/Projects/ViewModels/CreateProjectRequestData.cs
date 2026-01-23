using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Projects.ViewModels
{
    public class CreateProjectRequestData
    {
        public int InstitutionId { get; set; } = -1;
        public string Username { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTill { get; set; } = DateTime.Today;
        public string AdditionalInfo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
