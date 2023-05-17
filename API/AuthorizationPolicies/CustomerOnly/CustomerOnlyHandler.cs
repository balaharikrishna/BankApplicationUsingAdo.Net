using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.CustomerOnly
{
    public class CustomerOnlyHandler : AuthorizationHandler<CustomerOnlyRequirement>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public CustomerOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerOnlyRequirement requirement)
        {
            List<string> acceptedRoles = new() { "Customer" };
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
