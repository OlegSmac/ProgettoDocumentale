using System.Security.Claims;
using System.Web;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId
        {
            get
            {
                var principal = HttpContext.Current?.User as ClaimsPrincipal;
                if (principal?.Identity?.IsAuthenticated == false) return 0;
                
                var idValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(idValue, out var id) ? id : 0;
            }
        }

        public bool IsAuthenticated => (HttpContext.Current?.User?.Identity?.IsAuthenticated) == true;
    }
}
