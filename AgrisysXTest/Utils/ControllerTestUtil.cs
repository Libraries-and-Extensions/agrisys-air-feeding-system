using System.Security.Claims;
using System.Security.Principal;
using AgrisysAirFeedingSystem.Controllers.api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgrisysXTest.Utils;

public static class ControllerTestUtil
{
    public static void SetControllerContext(ControllerBase controller,string username,string role,string id = "2",List<Claim>? claims = null)
    {
        var context = new DefaultHttpContext();
        
        if (claims == null)
        {
           claims = new List<Claim>()
           {
               new Claim(ClaimTypes.Name, username),
               new Claim(ClaimTypes.Role, role),
               new Claim(ClaimTypes.NameIdentifier, id),
           };
        }
        else
        {
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, id),
            });

        }
        
        // create a user with the role Admin
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity(username) 
            ,claims));
            
        controller.ControllerContext.HttpContext = context;
    }
    
    
    public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls,List<string> roles) 
        where TUser : IdentityUser 
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
        mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        
        mgr.Setup(x =>x.Users).Returns(ls.AsQueryable());
        
        mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string id) => ls.FirstOrDefault(x => x.Id == id));
        mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((string name) => ls.FirstOrDefault(x => x.UserName == name));
        mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(true);
        mgr.Setup(x => x.GetRolesAsync(It.IsAny<TUser>())).ReturnsAsync((roles));
        
        mgr
            .Setup(x=>x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ClaimsPrincipal principal) => ls.FirstOrDefault(x =>
                x.Id == principal.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier)?.Value));

        
        mgr.Setup(x=>x.AddToRoleAsync(It.IsAny<TUser>(),It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        mgr.Setup(x=>x.RemoveFromRoleAsync(It.IsAny<TUser>(),It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        return mgr;
    }
}