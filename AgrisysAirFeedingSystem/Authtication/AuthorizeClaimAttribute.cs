using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Authtication;

public class AuthorizeClaimAttribute:TypeFilterAttribute
{
    public AuthorizeClaimAttribute(string claim, params string[]? values):base(typeof(SimpleClaimAuthFilter))
    {
        Arguments = new object[] { claim,values};
    }
}