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

        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {               
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via Mediatr" + ex);
            }
        }

        #region Views

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
        public ActionResult AddInstitution(CancellationToken cancellationToken)
        {                     
            return PartialView("_AddInstitutionModal", new CreateInstitutionRequestData());
        }

        [HttpGet]
        public async Task<ActionResult> UpdateInstitution(int id, CancellationToken cancellationToken)
        {
            var institution = await _mediator.Send(new GetInstitutionByIdQuery { Id = id }, cancellationToken);
            if (institution == null) return HttpNotFound();            

            var model = new UpdateInstitutionRequestData
            {
                Id = institution.Id,
                InstCode = institution.InstCode,
                Name = institution.Name,
                AdditionalInfo = institution.AdditionalInfo
            };

            return PartialView("_UpdateInstitutionModal", model);
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
        public async Task<ActionResult> AddUser(CancellationToken cancellationToken)
        {          
            var institutions = await _mediator.Send(new GetInstitutionsIdAndNameQuery(), cancellationToken);           
            var roles = await _mediator.Send(new GetRolesIdAndNameQuery(), cancellationToken);

            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");
            ViewBag.AvailableRoles = roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            return PartialView("_AddUserModal", new CreateUserRequestData());
        }

        [HttpGet]
        public async Task<ActionResult> UpdateUser(int id, CancellationToken cancellationToken)
        {
            await LoadInstitutionsAndRolesAsync(cancellationToken);

            var user = await _mediator.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
            if (user == null) return HttpNotFound();

            var model = new UpdateUserRequestData
            {                
                Id = user.Id,
                InstitutionId = user.InstitutionId,
                UserName = user.UserName,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Roles = user.Roles?.ToList() ?? new List<string>()
            };

            return PartialView("_UpdateUserModal", model);
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(int id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
            if (user == null) return HttpNotFound();

            var model = new ResetPasswordData
            {
                UserName = user.UserName,
                OldPassword = string.Empty,
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
            var output = await _mediator.Send(new RemoveInstitutionByIdCommand
            {
                Id = id
            }, cancellationToken);

            if (output != "removed") return Json(new { success = false, message = output });

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

            var output = await _mediator.Send(new SetUserAvailabilityCommand
            {
                UserId = id,
                IsEnable = !user.IsEnabled
            }, cancellationToken);

            if (output != "changed") return Json(new { success = false, message = output });

            return Json(new { success = true });
        }

        #endregion

        #endregion

        #region Helpers

        private async Task LoadInstitutionsAndRolesAsync(CancellationToken cancellationToken)
        {
            var institutions = await _mediator.Send(new GetInstitutionsIdAndNameQuery(), cancellationToken);        
            var roles = await _mediator.Send(new GetRolesIdAndNameQuery(), cancellationToken);

            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");
            ViewBag.AvailableRoles = roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
        }

        #endregion

    }
}