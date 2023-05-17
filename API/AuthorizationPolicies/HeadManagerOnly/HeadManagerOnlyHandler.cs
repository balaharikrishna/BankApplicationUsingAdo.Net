using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.HeadManagerOnly
{
    public class HeadManagerOnlyHandler : AuthorizationHandler<HeadManagerOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public HeadManagerOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HeadManagerOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "HeadManager" };
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
