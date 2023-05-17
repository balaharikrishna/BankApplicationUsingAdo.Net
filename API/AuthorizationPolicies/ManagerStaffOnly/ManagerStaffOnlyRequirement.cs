using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ManagerStaffOnly
{
    public class ManagerStaffOnlyRequirement : IAuthorizationRequirement
    {
    }
}
