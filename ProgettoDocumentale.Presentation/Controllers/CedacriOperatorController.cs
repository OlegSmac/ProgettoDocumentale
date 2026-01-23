using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using MediatR;
using ProgettoDocumentale.Application.Common.TableParameters;
using ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutionBy;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions;
using ProgettoDocumentale.Application.Requests.Projects.Commands;
using ProgettoDocumentale.Application.Requests.Projects.DTOs;
using ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjectBy;
using ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjects;
using ProgettoDocumentale.Application.Requests.Projects.ViewModels;
using ProgettoDocumentale.Application.Requests.Roles.Queries;

namespace ProgettoDocumentale.Presentation.Controllers
{
    [Authorize(Roles = "CedacriOperator")]
    public class CedacriOperatorController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IValidator<CreateProjectRequestData> _createProjectValidator;
        private readonly IValidator<UpdateProjectRequestData> _updateProjectValidator;

        public CedacriOperatorController(IMediator mediator,
            IValidator<CreateProjectRequestData> createProjectValidator,
            IValidator<UpdateProjectRequestData> updateProjectValidator)
        {
            _mediator = mediator;
            _createProjectValidator = createProjectValidator;
            _updateProjectValidator = updateProjectValidator;
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

        #region Documents

        [HttpPost]
        public async Task<ActionResult> DocumentsTable(DataTableParameters parameters, CancellationToken cancellationToken)
        {
            var documents = await _mediator.Send(new GetPagedDocumentsQuery
            {
                Parameters = parameters
            }, cancellationToken);

            return Json(new
            {
                draw = parameters.Draw,
                recordsFiltered = parameters.TotalCount,
                recordsTotal = parameters.TotalCount,
                data = documents
            });
        }

        #endregion

        #region Projects

        [HttpPost]
        public async Task<ActionResult> ProjectsTable(DataTableParameters parameters, int? institutionId, int? year, CancellationToken cancellationToken)
        {
            var projects = await _mediator.Send(new GetPagedProjectsQuery
            {
                Parameters = parameters,
                InstitutionId = institutionId,
                Year = year
            }, cancellationToken);

            return Json(new
            {
                draw = parameters.Draw,
                recordsFiltered = parameters.TotalCount,
                recordsTotal = parameters.TotalCount,
                data = projects
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectsHierarchy(CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetProjectsHierarchyQuery(), cancellationToken);
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> GetAddProject(CancellationToken cancellationToken)
        {
            await LoadInstitutionsAsync(cancellationToken);

            return PartialView("_AddProjectModal", new CreateProjectRequestData());
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateProject(int id, CancellationToken cancellationToken)
        {            
            await LoadInstitutionsAsync(cancellationToken);

            var project = await _mediator.Send(new GetProjectByIdQuery { Id = id }, cancellationToken);
            if (project == null) return HttpNotFound();

            var institution = await _mediator.Send(new GetInstitutionByIdQuery { Id = project.InstitutionId }, cancellationToken);

            var model = new UpdateProjectRequestData
            {
                Id = project.Id,
                InstitutionId = project.InstitutionId,
                Username = User.Identity?.Name,
                Name = project.Name,                
                DateFrom = project.DateFrom,
                DateTill = project.DateTill,
                AdditionalInfo = project.AdditionalInfo
            };

            return PartialView("_UpdateProjectModal", model);
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectDetails(int id, CancellationToken cancellationToken)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery { Id = id }, cancellationToken);
            if (project == null) return HttpNotFound();

            var institution = await _mediator.Send(new GetInstitutionByIdQuery { Id = project.InstitutionId }, cancellationToken);

            var model = new ProjectDTO
            {
                Id = project.Id,
                InstitutionId = project.InstitutionId,
                InstitutionName = project.InstitutionName,
                Username = User.Identity?.Name,
                Name = project.Name,
                DateFrom = project.DateFrom,
                DateTill = project.DateTill,
                AdditionalInfo = project.AdditionalInfo,
                IsActive = project.IsActive
            };

            return PartialView("_ProjectDetailsModal", model);
        }

        #endregion

        #endregion

        #region Operations

        #region Documents

        #endregion

        #region Projects

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProject(CreateProjectRequestData data, CancellationToken cancellationToken)
        {
            data.Username = User.Identity?.Name;
            await LoadInstitutionsAsync(cancellationToken);

            var validationResult = _createProjectValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_AddProjectModal", data);
            }

            try
            {                
                await _mediator.Send(new CreateProjectCommand
                {
                    ProjectRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_AddProjectModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_AddProjectModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProject(UpdateProjectRequestData data, CancellationToken cancellationToken)
        {            
            await LoadInstitutionsAsync(cancellationToken);

            var validationResult = _updateProjectValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_UpdateProjectModal", data);
            }

            try
            {
                var updated = await _mediator.Send(new UpdateProjectCommand
                {
                    ProjectRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_UpdateProjectModal", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_UpdateProjectModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveProject(int id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new RemoveProjectByIdCommand
            {
                Id = id
            }, cancellationToken);

            if (output != "removed") return Json(new { success = false, message = output });

            return Json(new { success = true });
        }

        #endregion

        #endregion

        #region Helpers

        private async Task LoadInstitutionsAsync(CancellationToken cancellationToken)
        {
            var institutions = await _mediator.Send(new GetInstitutionsIdAndNameQuery(), cancellationToken);            
            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");            
        }

        #endregion
    }
}