using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProgettoDocumentale.Application.Requests.Institutions.Commands;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;

namespace ProgettoDocumentale.API.Controllers
{
    [Route("api/institutions")]
    [ApiController]
    public class InstitutionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstitutionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstitutions()
        {            
            var result = await _mediator.Send(new GetAllInstitutionsQuery());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddInstitutionsList(List<CreateInstitutionRequestData> institutions)
        {
            if (institutions == null || institutions.Count == 0)
            {
                return BadRequest("Request body must contain a non-empty JSON array of institutions.");
            }

            await _mediator.Send(new AddInstitutionsListCommand { Institutions = institutions });

            return Ok();
        }
    }
}
