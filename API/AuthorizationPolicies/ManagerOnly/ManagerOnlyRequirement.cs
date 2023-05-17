using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ManagerOnly
{
    public class ManagerOnlyRequirement : IAuthorizationRequirement
    {
    }
}
