using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Common.Mappers;
using ProgettoDocumentale.Application.Requests.Projects.ViewModels;

namespace ProgettoDocumentale.Application.Requests.Projects.Queries.GetProjectBy
{
    public class GetUpdateProjectByIdQuery : IRequest<UpdateProjectRequestData>
    {
        public int Id { get; set; }
    }

    public class GetUpdateProjectByIdQueryHandler : IRequestHandler<GetUpdateProjectByIdQuery, UpdateProjectRequestData>
    {
        private readonly IProgettoDocContext _context;

        public GetUpdateProjectByIdQueryHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UpdateProjectRequestData> Handle(GetUpdateProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Institution)
                    .Include(p => p.User)
                    .Select(p => new UpdateProjectRequestData
                    {
                        Id = p.Id,
                        InstitutionId = p.Institution.Id,
                        Username = p.User.UserName,
                        Name = p.Name,
                        DateFrom = p.DateFrom,
                        DateTill = p.DateTill,
                        AdditionalInfo = p.AdditionalInfo,
                        IsActive = p.IsActive
                    })
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (project == null) throw new Exception($"Project with id={request.Id} not found");

                return project;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
