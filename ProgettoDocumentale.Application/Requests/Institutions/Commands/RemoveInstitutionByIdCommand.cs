using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Application.Requests.Institutions.Commands
{
    public class RemoveInstitutionByIdCommand : IRequest<string>
    {
        public int Id { get; set; }
    }

    public class RemoveInstitutionByIdCommandHandler : IRequestHandler<RemoveInstitutionByIdCommand, string>
    {
        private readonly IProgettoDocContext _context;

        public RemoveInstitutionByIdCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(RemoveInstitutionByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Should be implemented removing functionality?

                var instUsers =  await _context.Users.Where(u => u.InstitutionId == request.Id).ToListAsync();
                foreach (var user in instUsers) _context.Users.Remove(user);

                var institution = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == request.Id);
                if (institution == null) return $"Institution with id = {request.Id} doesn't exist";

                _context.Institutions.Remove(institution);
                await _context.SaveChangesAsync(cancellationToken);

                return "removed";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
