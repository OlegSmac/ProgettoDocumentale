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
using ProgettoDocumentale.Application.Requests.Documents.ViewModels;

namespace ProgettoDocumentale.Application.Requests.Documents.Queries.GetDocumentBy
{
    public class GetUpdateDocumentById : IRequest<UpdateDocumentRequestData>
    {
        public int Id { get; set; }
    }

    public class GetUpdateDocumentByIdHandler : IRequestHandler<GetUpdateDocumentById, UpdateDocumentRequestData>
    {
        private readonly IProgettoDocContext _context;

        public GetUpdateDocumentByIdHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<UpdateDocumentRequestData> Handle(GetUpdateDocumentById request, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _context.Documents
                    .Include(d => d.User)
                    .Include(d => d.Type)
                    .Include(d => d.Institution)
                    .Include(d => d.Project)
                    .Select(d => new UpdateDocumentRequestData
                    {
                        Id = d.Id,
                        InstitutionId = d.InstitutionId,
                        UserId = d.User.Id,
                        Name = d.Name,
                        TypeId = d.TypeId,
                        AdditionalInfo = d.AdditionalInfo,
                        GroupingDate = d.GroupingDate,
                        ProjectId = d.ProjectId,
                    })
                    .FirstOrDefaultAsync(d => d.Id == request.Id);

                if (document == null) throw new Exception($"Document with id={request.Id} not found");

                return document;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
