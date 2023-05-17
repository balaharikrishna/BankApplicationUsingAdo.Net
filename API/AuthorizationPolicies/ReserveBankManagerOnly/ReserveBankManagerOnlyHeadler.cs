using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ReserveBankManagerOnly
{
    public class ReserveBankManagerOnlyHeadler : AuthorizationHandler<ReserveBankManagerOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ReserveBankManagerOnlyHeadler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReserveBankManagerOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "ReserveBankManager" };
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
