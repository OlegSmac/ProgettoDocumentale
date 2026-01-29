using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Users.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
        public string RolesDisplay => string.Join(", ", Roles);

    }
}
