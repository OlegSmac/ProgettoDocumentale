using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MediatR;

namespace ProgettoDocumentale.Presentation.Controllers
{
    [Authorize(Roles = "BankOperator")]
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
    }
}