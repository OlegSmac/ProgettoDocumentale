using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Users.DTOs
{
    public class UserDTO
    {
        public int Id;
        public int InstitutionId;
        public string UserName;
        public string PasswordHash;
        public string Email;
        public bool IsEnabled;
        public string Name;
        public string Surname;
        public string Patronymic;

        public List<string> Roles { get; set; } = new List<string>();
    }
}
