using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.HeadManagerOnly
{
    public class HeadManagerOnlyRequirement : IAuthorizationRequirement
    {
    }
}
