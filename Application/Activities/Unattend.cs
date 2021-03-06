using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public sealed class Unattend
    {
        public sealed class Command : IRequest
        {
            public Guid ActivityId { get; set; }
        }

        public sealed class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.ActivityId);
                if (activity == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Cound not find activity." });
                }

                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == _userAccessor.GetCurrentUsername());

                var attendance = await _context.UserActivities.SingleOrDefaultAsync(x => x.ActivityId == activity.Id && x.AppUserId == user.Id);
                if (attendance == null)
                {
                    return Unit.Value;
                }

                if (attendance.IsHost)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Attendance = "You can't remove yourself as a host." });
                }

                _context.UserActivities.Remove(attendance);

                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}