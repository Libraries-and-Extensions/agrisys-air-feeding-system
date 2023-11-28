using Microsoft.AspNetCore.Authorization;

namespace AgrisysAirFeedingSystem.Authtication;

public class SimpleClaimRequirement : IAuthorizationRequirement
{
    public SimpleClaimRequirement(string claimType, string[]? claimValue = null)
    {
        Claim = claimType;
        Values = claimValue;
    }

    public string Claim { get; private set; }
    public string[]? Values { get; set; }
}