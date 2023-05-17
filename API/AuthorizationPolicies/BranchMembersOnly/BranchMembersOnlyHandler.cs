using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.BranchMembersOnly
{
    public class BranchMembersOnlyHandler : AuthorizationHandler<BranchMembersOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public BranchMembersOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BranchMembersOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "Manager", "Staff", "Customer" };
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
