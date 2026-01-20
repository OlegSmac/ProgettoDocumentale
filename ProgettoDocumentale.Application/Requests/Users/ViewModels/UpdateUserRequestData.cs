using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Requests.Users.ViewModels
{
    public class UpdateUserRequestData
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; } = -1;
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}
