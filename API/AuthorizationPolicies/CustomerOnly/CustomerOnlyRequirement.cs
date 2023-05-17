using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.CustomerOnly
{
    public class CustomerOnlyRequirement : IAuthorizationRequirement
    {
    }
}
