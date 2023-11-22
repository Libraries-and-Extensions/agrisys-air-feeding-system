using Microsoft.AspNetCore.Authorization;

namespace AgrisysAirFeedingSystem.Authtication;

public class SimpleClaimRequirement : IAuthorizationRequirement
{
    public string Claim { get; private set; }
    public string[]? Values { get; set; }
    
    public SimpleClaimRequirement(string claimType, string[]? claimValue)
    {
        Claim = claimType;
        Values = claimValue;
    }
}