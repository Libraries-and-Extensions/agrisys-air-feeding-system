using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using AgrisysAirFeedingSystem.Controllers;
using AgrisysAirFeedingSystem.Models.viewModels;
using AgrisysXTest.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgrisysXTest.Controllers;

    public class UserControllerTest
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            var users = new List<IdentityUser>()
            {
                new IdentityUser() { Id = "1", UserName = "testuser" },
                new IdentityUser() { Id = "2", UserName = "testuser2" }
            };
            
            var roles = new List<string>()
            {
                "Admin"
            };
            
            // Set up the user manager mock
            _userManagerMock = ControllerTestUtil.MockUserManager(users,roles);

            // Set up the logger mock
            _loggerMock = new Mock<ILogger<UserController>>();
            
            // Build the service provider
            _controller = new UserController(_userManagerMock.Object);
        }

        [Theory]
        [InlineData(true, 4)]
        [InlineData(false,0)]
        public async Task TestPermission(bool hasAssignPermission,int expectedCount)
        {
            // Set up the user manager mock to return a list of users
            var roles = new List<string>()
            {
                "Admin"
            };
            
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>())).ReturnsAsync(roles);
            
            var context = new DefaultHttpContext();

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, "Admin"),
            };
            
            if (hasAssignPermission)
            {
                claims.Add(new Claim("role:assign", ""));
            }
            
            // User is logged in
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity("testuser") 
            ,claims));
            
            _controller.ControllerContext.HttpContext = context;


            // Call the GetUsers method on the controller and assert that it returns a list of users
            var result = await _controller.List();
            Assert.NotNull(result);

            if (result is ViewResult viewResult)
            {
                var model = viewResult?.Model as UserListViewModel;
                Assert.NotNull(model);
                Assert.IsType<UserListViewModel>(model);
                
                Assert.Equal(expectedCount,model.roleOptions.Count());
            }
        }
        
        [Theory]
        [InlineData("Admin",4)]
        [InlineData("Manager",2)]
        [InlineData("User",0)]
        public async Task TestList(string role,int roleCount)
        {
            // Set up the user manager mock to return a list of users
            var roles = new List<string>()
            {
                role
            };
            
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>())).ReturnsAsync(roles);
            
            var context = new DefaultHttpContext();
            // create a user with the role Admin
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity("testuser") 
            ,new [] {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("role:assign", "")
            }));
            
            _controller.ControllerContext.HttpContext = context;

            // Call the GetUsers method on the controller and assert that it returns a list of users
            var result = await _controller.List();
            Assert.NotNull(result);

            if (result is ViewResult viewResult)
            {
                var model = viewResult?.Model as UserListViewModel;
                Assert.NotNull(model);
                Assert.IsType<UserListViewModel>(model);

                Assert.Equal(roleCount,model.roleOptions.Count());
            }
        }


        [Fact]
        public async Task TestGetUsersNoPermission()
        {
            // Set up the user manager mock to return a list of users


            var roles = new List<string>()
            {
                "Admin"
            };

            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>())).ReturnsAsync(roles);


            var context = new DefaultHttpContext();

            // User is logged out
            context.User = new GenericPrincipal(
                new GenericIdentity(String.Empty),
                new string[0]
            );
        }
    }