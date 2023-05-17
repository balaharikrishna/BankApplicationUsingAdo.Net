using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ManagerOnly
{
    public class ManagerOnlyHandler : AuthorizationHandler<ManagerOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ManagerOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManagerOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "Manager" };
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
