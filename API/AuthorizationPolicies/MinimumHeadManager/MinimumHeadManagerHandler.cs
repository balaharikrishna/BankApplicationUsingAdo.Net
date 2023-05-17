using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.MinimumHeadManager
{
    public class MinimumHeadManagerHandler : AuthorizationHandler<MinimumHeadManagerRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public MinimumHeadManagerHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumHeadManagerRequirement requirement)
        {
            List<string> acceptedRoles = new() { "HeadManager", "ReserveBankManager" };
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
