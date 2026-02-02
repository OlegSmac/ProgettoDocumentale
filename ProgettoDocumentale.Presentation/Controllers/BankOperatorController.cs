using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediatR;
using ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocumentBy;
using ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocuments;

namespace ProgettoDocumentale.Presentation.Controllers
{
    [Authorize(Roles = "BancOperator")]
    public class BankOperatorController : Controller
    {
        private readonly IMediator _mediator;

        public BankOperatorController(IMediator mediator)
        {
            _mediator = mediator;
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

        [HttpGet]
        public ActionResult ServiceReportsTab() => PartialView("_ServiceReportsTab");

        [HttpGet]
        public ActionResult SlaReportsTab() => PartialView("_SlaReportsTab");

        [HttpGet]
        public ActionResult ProjectsReportsTab() => PartialView("_ProjectsReportsTab");

        [HttpGet]
        public async Task<ActionResult> GetDocumentDetails(int id, CancellationToken cancellationToken)
        {
            var document = await _mediator.Send(new GetDocumentByIdQuery { Id = id }, cancellationToken);
            if (document == null) return HttpNotFound();

            return PartialView("_DocumentDetailsModal", document);
        }

        #endregion

        #region GetReports

        [HttpGet]
        public async Task<ActionResult> GetServiceReportsWithDateHierarchy(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _mediator.Send(new GetServiceReportsHierarchyQuery(), cancellationToken);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetSLAReportsWithDateHierarchy(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _mediator.Send(new GetSLAReportsHierarchyQuery(), cancellationToken);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectsWithDocumentsWithDateHierarchy(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _mediator.Send(new GetProjectsWithDocumentsHierarchyQuery(), cancellationToken);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult> DownloadDocument(int id, CancellationToken cancellationToken)
        {
            var document = await _mediator.Send(new GetDocumentByIdQuery { Id = id });
            if (document == null) return HttpNotFound();

            var root = Server.MapPath(ConfigurationManager.AppSettings["UploadsRoot"]);
            var fullPath = Path.Combine(root, document.SavedPath ?? "");
            if (!System.IO.File.Exists(fullPath)) return HttpNotFound("File not found on disk");

            var downloadName = document.Name;
            if (string.IsNullOrWhiteSpace(Path.GetExtension(downloadName))) downloadName += Path.GetExtension(fullPath);

            var contentType = MimeMapping.GetMimeMapping(downloadName);

            return File(fullPath, contentType, downloadName);
        }
    }
}