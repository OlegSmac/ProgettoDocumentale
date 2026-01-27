using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using MediatR;
using ProgettoDocumentale.Application.Common.TableParameters;
using ProgettoDocumentale.Application.Requests.Documents.Commands;
using ProgettoDocumentale.Application.Requests.Documents.DTOs;
using ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocumentBy;
using ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments;
using ProgettoDocumentale.Application.Requests.Documents.ViewModels;
using ProgettoDocumentale.Application.Requests.DocumentTypes.Queries;
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
        private static readonly HashSet<string> AllowedExt = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".pdf", ".doc", ".docx", ".xlsx", ".png", ".jpg", ".jpeg" };
        private const string ServReportCode = "SERV_REPORT";
        private const string SlaReportCode = "SLA_REPORT";
        private const string ProgettazioneCode = "PROGETTAZIONE";
        private const int MaxBytes = 50 * 1024 * 1024;

        private readonly IMediator _mediator;

        private readonly IValidator<CreateProjectRequestData> _createProjectValidator;
        private readonly IValidator<UpdateProjectRequestData> _updateProjectValidator;

        private readonly IValidator<CreateDocumentRequestData> _createDocumentValidator;
        private readonly IValidator<UpdateDocumentRequestData> _updateDocumentValidator;     

        public CedacriOperatorController(IMediator mediator,
            IValidator<CreateProjectRequestData> createProjectValidator,
            IValidator<UpdateProjectRequestData> updateProjectValidator,
            IValidator<CreateDocumentRequestData> createDocumentValidator,
            IValidator<UpdateDocumentRequestData> updateDocumentValidator)
        {
            _mediator = mediator;
            _createProjectValidator = createProjectValidator;
            _updateProjectValidator = updateProjectValidator;
            _createDocumentValidator = createDocumentValidator;
            _updateDocumentValidator = updateDocumentValidator;
        }

        public ActionResult Index(CancellationToken cancellationToken)
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

        [HttpGet]
        public async Task<ActionResult> GetAddDocument(CancellationToken cancellationToken)
        {
            var model = new CreateDocumentRequestData();
            await LoadInstitutionsTypesProjectsAsync(model.MacroTypeId, null, null, cancellationToken);

            return PartialView("_AddDocumentModal", model);
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateDocument(int id, CancellationToken cancellationToken)
        {
            var document = await _mediator.Send(new GetDocumentByIdQuery { Id = id }, cancellationToken);
            if (document == null) return HttpNotFound();

            var institution = await _mediator.Send(new GetInstitutionByIdQuery { Id = document.InstitutionId }, cancellationToken);
            if (institution == null) return HttpNotFound("Project institution not found");          

            var model = new UpdateDocumentRequestData
            {
                Id = document.Id,
                InstitutionId = institution.Id,
                Username = User.Identity?.Name,                
                Name = document.Name,
                AdditionalInfo = document.AdditionalInfo,
                GroupingDate = document.GroupingDate
            };

            bool setType = await CheckAndSetDocumentType(model, document, cancellationToken);
            if (setType == false) return HttpNotFound("Document type not found");

            await LoadInstitutionsTypesProjectsAsync(model.MacroTypeId, model.MicroTypeId, model.ProjectId, cancellationToken);

            return PartialView("_UpdateDocumentModal", model);
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
            if (institution == null) return HttpNotFound("Project institution not found");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddDocument(CreateDocumentRequestData data, HttpPostedFileBase file, CancellationToken cancellationToken)
        {
            data.Username = User.Identity?.Name;

            await CheckAndSetDocumentType(data, cancellationToken);
            await LoadInstitutionsTypesProjectsAsync(data.MacroTypeId, data.MicroTypeId, data.ProjectId, cancellationToken);
            ValidateFile(file);

            var validationResult = _createDocumentValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }                
            }

            if (!ModelState.IsValid) return PartialView("_AddDocumentModal", data);

            try
            {
                var root = Server.MapPath(ConfigurationManager.AppSettings["UploadsRoot"]);
                Directory.CreateDirectory(root);

                var ext = Path.GetExtension(file.FileName);
                var storedFileName = $"{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(root, storedFileName);

                file.SaveAs(fullPath);
                data.SavedPath = storedFileName;

                await _mediator.Send(new CreateDocumentCommand
                {
                    DocumentRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_AddDocumentModal", new CreateDocumentRequestData());
            }
            catch (Exception e)
            {               
                ModelState.AddModelError("", e.Message);
                await LoadInstitutionsTypesProjectsAsync(data.MacroTypeId, data.MicroTypeId, data.ProjectId, cancellationToken);
                return PartialView("_AddDocumentModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateDocument(UpdateDocumentRequestData data, CancellationToken cancellationToken)
        {
            data.Username = User.Identity?.Name;

            await LoadInstitutionsTypesProjectsAsync(data.MacroTypeId, data.MicroTypeId, data.ProjectId, cancellationToken);

            var validationResult = _updateDocumentValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                foreach (var err in validationResult.Errors)
                {
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                }

                return PartialView("_UpdateDocumentModal", data);
            }

            try
            {
                var updated = await _mediator.Send(new UpdateDocumentCommand
                {
                    DocumentRequest = data
                }, cancellationToken);

                ViewBag.Success = true;
                return PartialView("_UpdateDocumentModal", data);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                await LoadInstitutionsTypesProjectsAsync(data.MacroTypeId, data.MicroTypeId, data.ProjectId, cancellationToken);
                return PartialView("_UpdateDocumentModal", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveDocument(int id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new RemoveDocumentByIdCommand
            {
                Id = id
            }, cancellationToken);

            if (output != "removed") return Json(new { success = false, message = output });

            return Json(new { success = true });
        }

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
            var institutions = await _mediator.Send(new GetProjectsIdAndNameQuery(), cancellationToken);            
            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");            
        }

        private async Task LoadInstitutionsTypesProjectsAsync(int macroTypeId, int? microTypeId, int? projectId, CancellationToken cancellationToken)
        {
            var institutions = await _mediator.Send(new GetInstitutionIdAndNameQuery(), cancellationToken);
            ViewBag.Institutions = new SelectList(institutions, "Id", "Name");            

            var projects = await _mediator.Send(new GetProjectsIdAndNameQuery(), cancellationToken);
            ViewBag.Projects = new SelectList(projects, "Id", "Name", projectId);

            var macroTypes = await _mediator.Send(new GetMacroDocumentTypesIdAndNameQuery(), cancellationToken);
            ViewBag.MacroTypes = new SelectList(macroTypes, "Id", "Name");
            ViewBag.MacroTypesRaw = macroTypes;

            if (macroTypeId > 0)
            {                
                var selected = macroTypes.FirstOrDefault(x => x.Id == macroTypeId);
                if (selected != null && selected.Code != SlaReportCode)
                {
                    var microTypes = await _mediator.Send(new GetDocumentMicroTypesByMacroIdAndNameQuery { MacroId = macroTypeId }, cancellationToken);
                    ViewBag.MicroTypes = new SelectList(microTypes, "Id", "Name", microTypeId);
                }
                else ViewBag.MicroTypes = new SelectList(Enumerable.Empty<SelectListItem>());                
            }
            else ViewBag.MicroTypes = new SelectList(Enumerable.Empty<SelectListItem>());            
        }

        [HttpGet]
        public async Task<ActionResult> GetMicroTypes(int macroTypeId, CancellationToken cancellationToken)
        {
            if (macroTypeId <= 0) return Json(Enumerable.Empty<object>(), JsonRequestBehavior.AllowGet);

            var microTypes = await _mediator.Send(new GetDocumentMicroTypesByMacroIdAndNameQuery { MacroId = macroTypeId }, cancellationToken);            
            var result = microTypes.Select(x => new { x.Id, x.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task CheckAndSetDocumentType(CreateDocumentRequestData data, CancellationToken cancellationToken)
        {
            var macroTypes = await _mediator.Send(new GetMacroDocumentTypesIdNameCodeQuery(), cancellationToken);
            var selectedMacro = macroTypes.FirstOrDefault(x => x.Id == data.MacroTypeId);

            bool isSla = selectedMacro?.Code == SlaReportCode;
            bool isPrj = selectedMacro?.Code == ProgettazioneCode;

            if (isSla)
            {
                data.TypeId = data.MacroTypeId;
                data.MicroTypeId = null;
                data.ProjectId = null;
            }
            else
            {
                data.TypeId = data.MicroTypeId ?? -1;
                if (!isPrj) data.ProjectId = null;
            }
        }

        public async Task<bool> CheckAndSetDocumentType(UpdateDocumentRequestData data, DocumentDTO document, CancellationToken cancellationToken)
        {
            var docType = await _mediator.Send(new GetDocumentTypeByIdQuery { Id = document.TypeId });
            if (docType == null) return false;

            bool isSla = docType.Code == SlaReportCode;
            bool isServ = docType.Code == ServReportCode;
            bool isPrj = docType.Code == ProgettazioneCode;           

            if (isSla)
            {
                data.TypeId = document.TypeId;
                data.MacroTypeId = document.TypeId;
                data.MicroTypeId = null;
                data.ProjectId = null;
            }
            else if (isServ)
            {
                var macroType = await _mediator.Send(new GetMacroTypeFromMicroQuery { Id = document.TypeId });

                data.TypeId = document.TypeId;
                data.MacroTypeId = macroType.Id;
                data.MicroTypeId = document.TypeId;
                data.ProjectId = null;
            }
            else
            {
                var macroType = await _mediator.Send(new GetMacroTypeFromMicroQuery { Id = document.TypeId });

                data.TypeId = document.TypeId;
                data.MacroTypeId = macroType.Id;
                data.MicroTypeId = document.TypeId;
                data.ProjectId = document.ProjectId;
            }

            return true;
        }

        public void ValidateFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
            {
                ModelState.AddModelError(nameof(file), "Please select a file.");
            }
            else
            {
                if (file.ContentLength > MaxBytes) ModelState.AddModelError(nameof(file), "Max file size is 50 MB.");

                var ext = (Path.GetExtension(file.FileName) ?? "").ToLowerInvariant();
                if (!AllowedExt.Contains(ext)) ModelState.AddModelError(nameof(file), "Allowed formats: pdf, doc/docx, xlsx, png, jpg.");
            }
        }

        #endregion
    }
}