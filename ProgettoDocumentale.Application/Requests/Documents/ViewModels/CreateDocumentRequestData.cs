using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Documents.ViewModels
{
    public class CreateDocumentRequestData
    {
        public int InstitutionId { get; set; } = -1;
        public string Username { get; set; }
        public int MacroTypeId { get; set; } = -1;
        public int? MicroTypeId { get; set; }
        public int TypeId { get; set; } = -1;
        public int? ProjectId { get; set; } = -1;
        public string Name { get; set; }
        public string SavedPath { get; set; }
        public HttpPostedFileBase File { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string AdditionalInfo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GroupingDate { get; set; } = DateTime.Now;
    }
}
