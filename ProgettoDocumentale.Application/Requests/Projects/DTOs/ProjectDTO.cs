using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoDocumentale.Domain.Models;

namespace ProgettoDocumentale.Application.Requests.Projects.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsActive { get; set; }
    }
}
