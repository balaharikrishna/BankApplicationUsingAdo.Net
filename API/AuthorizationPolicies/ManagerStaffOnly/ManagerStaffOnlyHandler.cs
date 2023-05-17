using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ManagerStaffOnly
{
    public class ManagerStaffOnlyHandler : AuthorizationHandler<ManagerStaffOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ManagerStaffOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManagerStaffOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "Manager", "Staff" };
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
