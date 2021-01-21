using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Infrastructure.Security
{
    public sealed class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public sealed class IsHostRequirementHanler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public IsHostRequirementHanler(
            IHttpContextAccessor httpContextAccessor,
            DataContext context,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsHostRequirement requirement)
        {
            var currentUserName = _httpContextAccessor.HttpContext.User?.Claims?
                .SingleOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            var activityId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues["id"].ToString());

            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == activityId);
            var host = activity.UserActivities.FirstOrDefault(ua => ua.IsHost);

            if (host?.AppUser?.UserName == currentUserName)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}