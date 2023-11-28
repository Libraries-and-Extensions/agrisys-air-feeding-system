using Microsoft.AspNetCore.Authorization;

namespace AgrisysAirFeedingSystem.Authtication;

public class SimpleClaimAuthHandler : AuthorizationHandler<SimpleClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SimpleClaimRequirement requirement)
    {
        //check 
        var claims = context.User.Claims
            .Where(claim => claim.Type == requirement.Claim &&
                            (requirement.Values == null || requirement.Values.Length == 0
                                                        || requirement.Values.Contains(claim.Value)));

        if (claims.Any())
            context.Succeed(requirement);
        else
            context.Fail(new AuthorizationFailureReason(this, "User does not have the required claim"));

        return Task.CompletedTask;
    }
}