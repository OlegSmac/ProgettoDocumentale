using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        int RoleId { get; set; }
        bool IsAuthenticated { get; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
