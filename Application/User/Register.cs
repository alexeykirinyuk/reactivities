using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public sealed class Register
    {
        public sealed class Command : IRequest<User>
        {
            public string DisplayName { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(DataContext context)
            {
                RuleFor(c => c.DisplayName).NotEmpty();
                RuleFor(c => c.Username).NotEmpty()
                    .MustAsync(async (username, token) =>
                            !await context.Users.AnyAsync(u => u.UserName == username, token))
                    .WithMessage("Username already exists.");
                RuleFor(c => c.Email).NotEmpty().EmailAddress()
                    .MustAsync(async (email, token) =>
                        !await context.Users.AnyAsync(u => u.Email == email, token))
                    .WithMessage("Email already exists.");

                RuleFor(c => c.Password).NotEmpty().Password();
            }
        }

        public sealed class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(
                DataContext context,
                UserManager<AppUser> userManager,
                IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    UserName = request.Username,
                    Email = request.Email
                };

                var identityResult = await _userManager.CreateAsync(user, request.Password);

                if (identityResult.Succeeded)
                {
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Username = user.UserName,
                        Image = user.Photos.FirstOrDefault(u => u.IsMain)?.Url,
                        Token = _jwtGenerator.CreateToken(user)
                    };
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}