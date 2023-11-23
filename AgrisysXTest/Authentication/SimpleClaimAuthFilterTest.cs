using System.Security.Claims;
using System.Security.Principal;
using AgrisysAirFeedingSystem.Authtication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;

namespace AgrisysXTest.Authentication;

public class SimpleClaimAuthFilterTest
{
    [Fact]
    public async void TestFail()
    {
        
        
        
        Mock<IAuthorizationService> mockAuthorizationService = new();
        Mock<AuthorizationFilterContext> mockAuthorizationFilterContext = new();
        Mock<HttpContext> mockHttpContext = new();
        Mock<ClaimsPrincipal> mockClaimsPrincipal = new();
        Mock<IIdentity> mockIdentity = new();

        Claim[] claims = new Claim[]
        {
            new Claim("privacy", "")
        };
        
        
        mockIdentity.SetupGet((principal => principal.IsAuthenticated)).Returns(true);
        
        mockClaimsPrincipal.SetupGet((principal => principal.Identity)).Returns(mockIdentity.Object);
        
        mockClaimsPrincipal.SetupGet((principal => principal.Claims)).Returns(claims.AsEnumerable());
        
        mockHttpContext.SetupGet((http => http.User)).Returns(mockClaimsPrincipal.Object);
        
        mockAuthorizationFilterContext.SetupGet((filterContext => filterContext.HttpContext))
            .Returns(mockHttpContext.Object);

        mockAuthorizationFilterContext.SetupSet((filterContext => filterContext.Result = It.IsAny<ChallengeResult>())).Verifiable();
        
        SimpleClaimAuthFilter subject = new(mockAuthorizationService.Object, "privacy");
        
        
        AuthorizationFilterContext context;
        /*var user = context.HttpContext.User;

        if (!user.Identity!.IsAuthenticated)
        {
            context.Result = new ChallengeResult();
            return;
        }


        var result = await _authorization.AuthorizeAsync(user, null, _requirement);

        if (!result.Succeeded)
        {
            context.Result = new ForbidResult();
        }*/
    }
    
}