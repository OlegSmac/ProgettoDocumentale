using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoDocumentale.Application.Requests.Institutions.Commands;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions;
using ProgettoDocumentale.Application.Requests.Institutions.ViewModels;
using ProgettoDocumentale.Application.Requests.Projects.Commands;
using ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjects;
using ProgettoDocumentale.Application.Requests.Projects.ViewModels;

namespace ProgettoDocumentale.API.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _mediator.Send(new GetAllProjectsQuery());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectsList(List<CreateProjectRequestData> projects)
        {
            if (projects == null || projects.Count == 0)
            {
                return BadRequest("Request body must contain a non-empty JSON array of projects.");
            }

            await _mediator.Send(new AddProjectsListCommand { Projects = projects });

            return Ok();
        }
    }
}
