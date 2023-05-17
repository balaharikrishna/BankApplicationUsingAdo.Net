using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.BranchMembersOnly
{
    public class BranchMembersOnlyRequirement : IAuthorizationRequirement
    {
    }
}
