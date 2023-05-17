using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.StaffOnly
{
    public class StaffOnlyHandler : AuthorizationHandler<StaffOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public StaffOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StaffOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "Staff" };
            string role = _httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == "Role").Value ?? "";
            if (acceptedRoles.Contains(role))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
