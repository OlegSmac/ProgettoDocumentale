using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Domain.Models
{
    public class User : BaseEntity
    {
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public ICollection<UserToRole> UserRoles { get; set; } = new List<UserToRole>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

    }
}
