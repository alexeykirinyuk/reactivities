using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public sealed class Delete
    {
        public sealed class Command : IRequest
        {
            public string Id { get; set; }
        }

        public sealed class CommandHandler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public CommandHandler(
                DataContext context,
                IPhotoAccessor photoAccessor,
                IUserAccessor userAccessor)
            {
                _context = context;
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetCurrentUsername());

                var photo = user.Photos
                    .FirstOrDefault(p => p.Id == request.Id);

                if (photo == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { photo = "Photo not found." });

                if (photo.IsMain)
                    throw new RestException(HttpStatusCode.BadRequest, new { photo = "You can not delete main photo." });

                var result = await _photoAccessor.DeletePhoto(request.Id);
                if (result == null)
                {
                    throw new Exception("Problem deleting the photo.");
                }
                user.Photos.Remove(photo);

                var success = await _context.SaveChangesAsync() > 0;
                if (success)
                    return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}