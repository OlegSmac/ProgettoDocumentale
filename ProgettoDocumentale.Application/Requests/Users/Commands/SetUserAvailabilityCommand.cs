using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Application.Requests.Users.Commands
{
    public class SetUserAvailabilityCommand : IRequest<string>
    {
        public int UserId { get; set; }
        public bool IsEnable { get; set; }
    }
    
    public class SetUserAvailabilityCommandHandler : IRequestHandler<SetUserAvailabilityCommand, string>
    {
        private readonly IProgettoDocContext _context;

        public SetUserAvailabilityCommandHandler(IProgettoDocContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(SetUserAvailabilityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                if (user == null) return $"User with id={request.UserId} not found";

                user.IsEnabled = request.IsEnable;

                await _context.SaveChangesAsync(cancellationToken);

                return "changed";
            }
            catch (Exception e)
            {
                throw new Exception("SetUserAvailabilityCommand exception " + e.Message);
            }
        }
    }
}
