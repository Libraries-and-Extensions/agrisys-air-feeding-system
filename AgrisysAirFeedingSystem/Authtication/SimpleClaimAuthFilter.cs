using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgrisysAirFeedingSystem.Authtication;


public class SimpleClaimAuthFilter: IAsyncAuthorizationFilter
{
    private readonly IAuthorizationService _authorization;
    
    public SimpleClaimRequirement _requirement { get; private set; }
    
    public SimpleClaimAuthFilter(IAuthorizationService authorization,string claim, params string[]? values)
    {
        _authorization = authorization;
       _requirement = new SimpleClaimRequirement(claim, values);
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity!.IsAuthenticated)
        {
            context.Result = new ChallengeResult();
            return;
        }

        var result = await _authorization.AuthorizeAsync(user, null, _requirement);

         if (!result.Succeeded)
         {
             context.Result = new ForbidResult();
         }
    }
}