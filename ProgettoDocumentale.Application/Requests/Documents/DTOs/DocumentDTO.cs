using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProgettoDocumentale.Application.Requests.Documents.DTOs
{
    public class DocumentDTO
    {
        public int Id { get; set; }

        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }

        public string Username { get; set; }

        public int TypeId { get; set; } 
        public string TypeName { get; set; }

        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string Name { get; set; }
        public string SavedPath { get; set; }
        
        public DateTime UploadDate { get; set; }
        public string UploadDateString
        {
            get => UploadDate.ToShortDateString();
        }

        public string AdditionalInfo { get; set; }
        
        public DateTime GroupingDate { get; set; }
        public string GroupingDateString
        {
            get => GroupingDate.ToShortDateString();
        }
        
    }
}
