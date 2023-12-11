using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using AgrisysAirFeedingSystem.Controllers.api;
using AgrisysAirFeedingSystem.Models.apiModels;
using AgrisysAirFeedingSystem.Models.Enums;
using AgrisysAirFeedingSystem.Models.Extra;
using AgrisysXTest.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgrisysXTest.Controllers;

public class UserApiTest
{
    private UserApiController _controller;
    private Mock<UserManager<IdentityUser>> _userManagerMock;
    private List<IdentityUser> _users;
    private List<string> _roles;

    public UserApiTest()
    {
        _users = new List<IdentityUser>()
        {
            new IdentityUser() { Id = "1", UserName = "testuser" },
            new IdentityUser() { Id = "2", UserName = "testuser2" }
        };
        
        _roles = new List<string>()
        {
            "Admin"
        };
    }

    [Theory]
    [InlineData("1",HttpStatusCode.NoContent)]
    [InlineData("3",HttpStatusCode.NotFound)]
    public async Task TestFindID(string id,HttpStatusCode code)
    {
        _userManagerMock = ControllerTestUtil.MockUserManager(_users,_roles);
        _controller = new UserApiController(_userManagerMock.Object);
        
        ControllerTestUtil.SetControllerContext(_controller,"testuser","Admin");

        
        // Call the ChangeRole method on the controller and assert that it returns a single user
        var result = await _controller.ChangeRole(new ChangeRoleModel()
        {
            Id = id,
            Role = "None",
            OldRole = "None"
        });
        
        assertApiRepsonse(result,code);
    }
    
    [Theory]
    [InlineData(RoleEnum.User,RoleEnum.Manager,RoleEnum.User,HttpStatusCode.Forbidden)]
    [InlineData(RoleEnum.User,RoleEnum.Admin,RoleEnum.User,HttpStatusCode.Forbidden)]
    [InlineData(RoleEnum.Manager,RoleEnum.Admin,RoleEnum.Manager,HttpStatusCode.Forbidden)]
    [InlineData(RoleEnum.Manager,RoleEnum.Admin,RoleEnum.User,HttpStatusCode.Forbidden)]
    [InlineData(RoleEnum.Manager,RoleEnum.None,RoleEnum.User,HttpStatusCode.NoContent)]
    [InlineData(RoleEnum.Manager,RoleEnum.User,RoleEnum.None,HttpStatusCode.NoContent)]
    [InlineData(RoleEnum.Admin,RoleEnum.Admin,RoleEnum.User,HttpStatusCode.NoContent)]
    [InlineData(RoleEnum.Admin,RoleEnum.Admin,RoleEnum.Manager,HttpStatusCode.NoContent)]
    [InlineData(RoleEnum.Admin,RoleEnum.Manager,RoleEnum.Admin,HttpStatusCode.NoContent)]
    [InlineData(RoleEnum.Admin,RoleEnum.User,RoleEnum.Manager,HttpStatusCode.NoContent)]
    public async Task TestRolePermission(RoleEnum userRole,RoleEnum role,RoleEnum oldRole,HttpStatusCode code)
    {
        _roles = new List<string>()
        {
            userRole.ToString()
        };

        _userManagerMock = ControllerTestUtil.MockUserManager(_users,_roles);
        _controller = new UserApiController(_userManagerMock.Object);
        
        ControllerTestUtil.SetControllerContext(_controller,"testuser",userRole.ToString());
        
        // Call the ChangeRole method on the controller and assert that it returns a single user
        var result = await _controller.ChangeRole(new ChangeRoleModel()
        {
            Id = "1",
            Role = role.ToString(),
            OldRole = oldRole.ToString()
        });

        assertApiRepsonse(result,code);
    }
    [Theory]
    [InlineData(2,1,HttpStatusCode.NoContent)]
    [InlineData(1,1,HttpStatusCode.Forbidden)]
    public async Task TestPreventOwnRoleChangePermission(int Authid,int targeIid,HttpStatusCode code)
    {
        var userManagerMock = ControllerTestUtil.MockUserManager(_users,_roles);
        var controller = new UserApiController(userManagerMock.Object);
        ControllerTestUtil.SetControllerContext(controller,_users[Authid-1].UserName!,"Admin",id:_users[Authid-1].Id);

        /*var context = new DefaultHttpContext();
        // create a user with the role Admin
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity(users[id-1].UserName!)
            ,new [] {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, users[id-1].Id),
            }));
            
        controller.ControllerContext.HttpContext = context;*/
        
        // Call the ChangeRole method on the controller and assert that it returns a single user
        var result = await controller.ChangeRole(new ChangeRoleModel()
        {
            Id = targeIid.ToString(),
            Role = "None",
            OldRole = "None"
        });

        assertApiRepsonse(result,code);
    }
    
    private void assertApiRepsonse(IActionResult result, HttpStatusCode code)
    {
        if (code == HttpStatusCode.NoContent)
        {
            Assert.IsType<StatusCodeResult>(result);
            
            var resporse = (StatusCodeResult) result;
        
            Assert.Equal((int)code,resporse.StatusCode);
        }
        else
        {
            Assert.IsType<ErrorResponse>(result);
            
            var resporse = (ErrorResponse) result;
        
            Assert.Equal((int)code,resporse.StatusCode);
        }
    }
}