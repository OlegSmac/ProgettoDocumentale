using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FluentValidation;
using MediatR;
using Microsoft.Owin.Security;
using ProgettoDocumentale.Application.Requests.Users.Queries.GetUserBy;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;

namespace ProgettoDocumentale.Presentation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private IAuthenticationManager authenticationManager => HttpContext.GetOwinContext().Authentication;
        private readonly IValidator<LoginUser> _loginValidator;        

        public AccountController(IMediator mediator, IValidator<LoginUser> loginValidator)
        {
            _mediator = mediator;
            _loginValidator = loginValidator;            
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginUser model)
        {
            var validatorResponse = _loginValidator.Validate(model);

            if (!validatorResponse.IsValid)
            {
                foreach (var error in validatorResponse.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            else
            {

                var user = await _mediator.Send(new GetUserByUsernameAndPasswordQuery
                {
                    UserName = model.UserName,
                    Password = model.Password,
                });

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid Username and Password");
                    return View();                    
                }
                else if (user.IsEnabled == false)
                {
                    ModelState.AddModelError("", "Your account is Deactivated.");
                }
                else
                {
                    ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                    claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
                    claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "OWIN Provider", ClaimValueTypes.String));

                    foreach (string role in user.Roles)
                    {
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role, ClaimValueTypes.String));
                    }

                    authenticationManager.SignOut();
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    if (user.Roles.Contains("Admin")) return RedirectToAction("Index", "Admin");                    

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            authenticationManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
    }
}