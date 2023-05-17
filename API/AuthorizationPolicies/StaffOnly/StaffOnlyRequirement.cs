using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.StaffOnly
{
    public class StaffOnlyRequirement : IAuthorizationRequirement
    {
    }
}
