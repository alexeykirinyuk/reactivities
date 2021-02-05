using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public sealed class SetMain
    {
        public sealed class Command : IRequest
        {
            public string Id { get; set; }
        }

        public sealed class CommandHandler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public CommandHandler(
                DataContext context,
                IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetCurrentUsername());

                var photo = user.Photos
                    .FirstOrDefault(p => p.Id == request.Id);

                if (photo == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Photo = "Not found" });

                if (photo.IsMain)
                    throw new RestException(HttpStatusCode.BadRequest, new { Photo = "Already main" });

                var currentMainPhoto = user.Photos
                    .FirstOrDefault(p => p.IsMain);

                photo.IsMain = true;
                currentMainPhoto.IsMain = false;

                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    throw new Exception("Problem saving data");

                return Unit.Value;
            }
        }
    }
}