using System.Security.Claims;
using System.Security.Principal;
using AgrisysAirFeedingSystem.Authtication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AgrisysXTest.Authentication;

public class SimpleClaimAuthFilterTest
{
    private AuthorizationFilterContext ContextHelper(string claim,bool auth)
    {
        Mock<IAuthorizationService> mockAuthorizationService = new();
        Mock<HttpContext> mockHttpContext = new();
        Mock<ClaimsPrincipal> mockClaimsPrincipal = new();
        Mock<IIdentity> mockIdentity = new();

        Claim[] claims = new Claim[]
        {
            new Claim(claim, "")    
        };
        
        mockIdentity.SetupGet((principal => principal.IsAuthenticated)).Returns(auth);
        
        mockClaimsPrincipal.SetupGet((principal => principal.Identity)).Returns(mockIdentity.Object);
        
        mockClaimsPrincipal.SetupGet((principal => principal.Claims)).Returns(claims.AsEnumerable());
        
        mockHttpContext.SetupGet((http => http.User)).Returns(mockClaimsPrincipal.Object);
        
        SimpleClaimAuthFilter subject = new(mockAuthorizationService.Object, "privacy");
        
        ActionContext fakeActionContext =
            new ActionContext(mockHttpContext.Object, 
                new Microsoft.AspNetCore.Routing.RouteData(), 
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        
        return new AuthorizationFilterContext(fakeActionContext,new List<IFilterMetadata>()) ;
    }
    
    [Fact]
    public async void TestSuccess()
    {
        var fakeAuthFilterContext = ContextHelper("privacy",true);
        
        IAuthorizationService service = BuildAuthorizationService(services =>
        {
            services.AddSingleton<IAuthorizationHandler, SimpleClaimAuthHandler>();
        });
        
        SimpleClaimAuthFilter subject = new(service, "privacy");
        
        await subject.OnAuthorizationAsync(fakeAuthFilterContext);
        
        Assert.Null(fakeAuthFilterContext.Result);
    }
    
    [Fact]
    public async void TestChallenge()
    {
        var fakeAuthFilterContext = ContextHelper("privacy",false);
        
        IAuthorizationService service = BuildAuthorizationService(services =>
        {
            services.AddSingleton<IAuthorizationHandler, SimpleClaimAuthHandler>();
        });
        
        SimpleClaimAuthFilter subject = new(service, "privacy");
        
        await subject.OnAuthorizationAsync(fakeAuthFilterContext);
        
        Assert.IsType<ChallengeResult>(fakeAuthFilterContext.Result);
    }
    
    [Fact]
    public async void TestForbid()
    {
        var fakeAuthFilterContext = ContextHelper("forbid",true);
        
        IAuthorizationService service = BuildAuthorizationService(services =>
        {
            services.AddSingleton<IAuthorizationHandler, SimpleClaimAuthHandler>();
        });
        
        SimpleClaimAuthFilter subject = new(service, "privacy");
        
        await subject.OnAuthorizationAsync(fakeAuthFilterContext);
        
        Assert.IsType<ForbidResult>(fakeAuthFilterContext.Result);
    }
    
    private IAuthorizationService BuildAuthorizationService(Action<IServiceCollection> setupServices = null)
    {
        var services = new ServiceCollection();
        services.AddAuthorization();
        services.AddLogging();
        services.AddOptions();
        setupServices?.Invoke(services);
        return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
    }
    
}