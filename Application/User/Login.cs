using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public sealed class Login
    {
        public sealed class Query : IRequest<User>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Email).NotEmpty();
                RuleFor(m => m.Password).NotEmpty();
            }
        }

        public sealed class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(
                UserManager<AppUser> userManager,
                 SignInManager<AppUser> signInManager,
                 IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result == SignInResult.Success)
                {
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Username = user.UserName,
                        Token = _jwtGenerator.CreateToken(user),
                        Image = user.Photos.FirstOrDefault(u => u.IsMain)?.Url
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
