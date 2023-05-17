using Microsoft.AspNetCore.Authorization;

namespace API.AuthorizationPolicies.ReserveBankManagerOnly
{
    public class ReserveBankManagerOnlyRequirement : IAuthorizationRequirement
    {
    }
}
