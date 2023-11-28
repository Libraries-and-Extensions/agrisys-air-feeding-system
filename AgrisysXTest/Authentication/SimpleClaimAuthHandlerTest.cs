using System.Security.Claims;
using AgrisysAirFeedingSystem.Authtication;
using Microsoft.AspNetCore.Authorization;

namespace AgrisysXTest.Authentication;

public class SimpleClaimAuthHandlerTest
{
    [Fact]
    public async void TestFail()
    {
        //Arrange    
        var requirements = new[] { new SimpleClaimRequirement("privacy") };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim("read", "")
                })
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var subject = new SimpleClaimAuthHandler();

        //Act
        await subject.HandleAsync(context);

        //Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async void TestSucces()
    {
        //Arrange    
        var requirements = new[] { new SimpleClaimRequirement("privacy") };
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim("privacy", "")
                })
        );
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var subject = new SimpleClaimAuthHandler();

        //Act
        await subject.HandleAsync(context);

        //Assert
        Assert.True(context.HasSucceeded);
    }
}