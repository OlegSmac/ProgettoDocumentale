using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FluentValidation;
using MediatR;
using ProgettoDocumentale.Application.Common.TableParameters;
using ProgettoDocumentale.Application.Requests.Institutions.Commands;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutionBy;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;
using ProgettoDocumentale.Application.Requests.Roles.Queries;
using ProgettoDocumentale.Application.Requests.Users.Commands;
using ProgettoDocumentale.Application.Requests.Users.Queries.GetUserBy;
using ProgettoDocumentale.Application.Requests.Users.Queries.GetUsers;
using ProgettoDocumentale.Application.Requests.Users.ViewModels;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace ProgettoDocumentale.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IValidator<CreateInstitutionRequestData> _createInstitutionValidator;
        private readonly IValidator<UpdateInstitutionRequestData> _updateInstitutionValidator;

        private readonly IValidator<CreateUserRequestData> _createUserValidator;
        private readonly IValidator<UpdateUserRequestData> _updateUserValidator;
        private readonly IValidator<ResetPasswordData> _resetPasswordValidator;

        public AdminController(IMediator mediator,
            IValidator<CreateInstitutionRequestData> createInstitutionValidator,
            IValidator<UpdateInstitutionRequestData> updateInstitutionValidator,
            IValidator<CreateUserRequestData> createUserValidator, 
            IValidator<UpdateUserRequestData> updateUserValidator,
            IValidator<ResetPasswordData> resetPasswordValidator)
        {
            _mediator = mediator;

            _createInstitutionValidator = createInstitutionValidator;
            _updateInstitutionValidator = updateInstitutionValidator;

            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _resetPasswordValidator = resetPasswordValidator;
        }

        [HttpGet]
        public ActionResult Index() => View();

        #region Views

        [HttpGet]
        public ActionResult InstitutionsTab() => PartialView("_InstitutionsTab");

        [HttpGet]
        public ActionResult UsersTab() => PartialView("_UsersTab");

        #region Institutions

        [HttpPost]
        public async Task<ActionResult> InstitutionTable(DataTableParameters parameters, CancellationToken cancellationToken)
        {
            var institutions = await _mediator.Send(new GetPagedInstitutionsQuery
            {
                Parameters = parameters
            }, cancellationToken);

            return Json(new
            {
                draw = parameters.Draw,
                recordsFiltered = parameters.TotalCount,
                recordsTotal = parameters.TotalCount,
                data = institutions
            });
        }

        [HttpGet]
        public ActionResult GetAddInstitution(CancellationToken cancellationToken)
        {                     
            return PartialView("_AddInstitutionModal", new CreateInstitutionRequestData());
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateInstitution(int id, CancellationToken cancellationToken)
        {
            var institutionModel = await _mediator.Send(new GetUpdateInstitutionByIdQuery { Id = id }, cancellationToken);
            if (institutionModel == null) return HttpNotFound();

            return PartialView("_UpdateInstitutionModal", institutionModel);
        }

        #endregion

        #region Users

        [HttpPost]
        public async Task<ActionResult> UserTable(DataTableParameters parameters, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetPagedUsersQuery
            {
                Parameters = parameters
            }, cancellationToken);

            return Json(new
            {
                draw = parameters.Draw,
                recordsFiltered = parameters.TotalCount,
                recordsTotal = parameters.TotalCount,
                data = users
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetAddUser(CancellationToken cancellationToken)
        {          
            var institutions = await _mediator.Send(new GetInstitutionIdAndNameQuery(), cancellationToken);           
            var roles = await _mediator.Send(new GetRolesIdAndNameQuery(), cancellationToken);

            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");

            ViewBag.AvailableRoles = roles
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                .ToList();

            return PartialView("_AddUserModal", new CreateUserRequestData());
        }

        [HttpGet]
        public async Task<ActionResult> GetUserDetails(int id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
            if (user == null) return HttpNotFound();

            return PartialView("_UserDetailsModal", user);
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateUser(int id, CancellationToken cancellationToken)
        {
            await LoadInstitutionsAndRolesAsync(cancellationToken);

            var userModel = await _mediator.Send(new GetUpdateUserByIdQuery { Id = id }, cancellationToken);
            if (userModel == null) return HttpNotFound();           

            return PartialView("_UpdateUserModal", userModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetResetPassword(int id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
            if (user == null) return HttpNotFound();

            var model = new ResetPasswordData
            {
                UserName = user.UserName,                
                NewPassword = string.Empty
            };

            return PartialView("_ResetPasswordModal", model);
        }

        #endregion

        #endregion

        #region Operations

        #region Institutions 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddInstitution(CreateInstitutionRequestData data, CancellationToken cancellationToken)
        {
            var validationResult = _createInstitutionValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_AddInstitutionModal", data);
            }

            try
            {
                await _mediator.Send(new CreateInstitutionCommand
                {
                    InstitutionRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_AddInstitutionModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_AddInstitutionModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateInstitution(UpdateInstitutionRequestData data, CancellationToken cancellationToken)
        {
            var validationResult = _updateInstitutionValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_UpdateInstitutionModal", data);
            }

            try
            {
                var updated = await _mediator.Send(new UpdateInstitutionCommand
                {
                    InstitutionRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_UpdateInstitutionModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_UpdateInstitutionModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveInstitution(int id, CancellationToken cancellationToken)
        {
            try
            {
                var output = await _mediator.Send(new RemoveInstitutionByIdCommand
                {
                    Id = id
                }, cancellationToken);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

            return Json(new { success = true });
        }

        #endregion

        #region Users

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(CreateUserRequestData data, CancellationToken cancellationToken)
        {
            await LoadInstitutionsAndRolesAsync(cancellationToken);

            var validationResult = _createUserValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_AddUserModal", data);
            }

            try
            {
                var created = await _mediator.Send(new CreateUserCommand
                {
                    UserRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_AddUserModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_AddUserModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(UpdateUserRequestData data, CancellationToken cancellationToken)
        {
            await LoadInstitutionsAndRolesAsync(cancellationToken);       

            var validationResult = _updateUserValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_UpdateUserModal", data);
            }

            try
            {
                var updated = await _mediator.Send(new UpdateUserCommand
                {
                    UserRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_UpdateUserModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_UpdateUserModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordData data, CancellationToken cancellationToken)
        {
            var validationResult = _resetPasswordValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_ResetPasswordModal", data);
            }

            try
            {
                var resetedPassword = await _mediator.Send(new UpdatePasswordCommand
                {
                    PasswordRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_ResetPasswordModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_ResetPasswordModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableDisableUser(int id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
            if (user == null) return HttpNotFound();

            try
            {
                var output = await _mediator.Send(new SetUserAvailabilityCommand
                {
                    UserId = id,
                    IsEnable = !user.IsEnabled
                }, cancellationToken);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

            return Json(new { success = true });
        }

        #endregion

        #endregion

        #region Helpers

        private async Task LoadInstitutionsAndRolesAsync(CancellationToken cancellationToken)
        {
            var institutions = await _mediator.Send(new GetInstitutionIdAndNameQuery(), cancellationToken);        
            var roles = await _mediator.Send(new GetRolesIdAndNameQuery(), cancellationToken);

            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");
            ViewBag.AvailableRoles = roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
        }

        #endregion

    }
}