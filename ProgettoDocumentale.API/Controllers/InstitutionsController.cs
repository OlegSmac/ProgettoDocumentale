using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions;

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
    }
}
