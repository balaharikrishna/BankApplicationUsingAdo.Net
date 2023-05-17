using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ManagerHeadManagerOnly
{
    public class ManagerHeadManagerOnlyHandler : AuthorizationHandler<ManagerHeadManagerOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ManagerHeadManagerOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManagerHeadManagerOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "HeadManager", "Manager" };
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
