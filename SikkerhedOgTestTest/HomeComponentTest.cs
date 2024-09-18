using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using SikkerhedOgTest;
using SikkerhedOgTest.Components.Pages;
using SikkerhedOgTest.Data;
using Xunit;

namespace SikkerhedOgTestTest;

public class HomeComponentTest
{

    UserManager<ApplicationUser> userManager { get; set; }
  

    public HomeComponentTest()
    {
        userManager = UserManagerHelper.CreateUserManager();
    }
    [Fact]
    public void Authorized_User_Gets_Authorized_Message_When_Logged_In()
    {
        // Arrange
        using var ctx = new TestContext(); // Initializing bUnit test context

        // Register UserManager instance in test services
        ctx.Services.AddSingleton(userManager);

        var testAuth = ctx.AddTestAuthorization();
        testAuth.SetAuthorized("testuser");
     

        // Render the Home component
        var cut = ctx.RenderComponent<Home>();

        // Act
        var authorizedMessage = cut.Find(".authorized-message");

        // Assert
        Assert.Equal("You are authorized!", authorizedMessage.TextContent);
    }

    [Fact]
    public void Unauthorized_User_Gets_Unauthorized_Message_When_Not_Logged_In()
    {
        // Arrange
        using var ctx = new TestContext(); // Initializing bUnit test context

        // Register UserManager instance in test services
        ctx.Services.AddSingleton(userManager);


        var testAuth = ctx.AddTestAuthorization();
        testAuth.SetNotAuthorized();

        // Render the Home component
        var cut = ctx.RenderComponent<Home>();

        // Act
        var unauthorizedMessage = cut.Find(".unauthorized-message");
        // Assert
        Assert.Equal("You are not logged in!", unauthorizedMessage.TextContent);
    }

    [Fact]
    public void Authorized_User_Gets_Authorized_And_User_Message_When_Logged_In_With_User_Role()
    {
        // Arrange
        using var ctx = new TestContext(); // Initializing bUnit test context

        // Register UserManager instance in test services
        ctx.Services.AddSingleton(userManager);

        var testAuth = ctx.AddTestAuthorization();
        testAuth.SetRoles("User");
        testAuth.SetAuthorized("testuser");

        // Render the Home component
        var cut = ctx.RenderComponent<Home>();

        // Act
        var authorizedUserMessage = cut.Find(".authorized-user");

        // Assert
        Assert.Equal("You are User!", authorizedUserMessage.TextContent);
    }

    [Fact]
    public void Authorized_User_Gets_Authorized__And_Admin_Message_When_Logged_In_With_Admin_Role()
    {
        // Arrange
        using var ctx = new TestContext(); // Initializing bUnit test context

        // Register UserManager instance in test services
        ctx.Services.AddSingleton(userManager);

        var testAuth = ctx.AddTestAuthorization();
        testAuth.SetRoles("Admin");
        testAuth.SetAuthorized("testuser");
 

        // Render the Home component
        var cut = ctx.RenderComponent<Home>();

        // Act
        var authorizedAdminMessage = cut.Find(".authorized-admin");

        // Assert
        Assert.Equal("You are Admin!", authorizedAdminMessage.TextContent);
    }

    [Fact]
    public void Authenticated_User_Should_Set_IsAuthenticated_Local_Variable_To_True()
    {
        // Arrange
        using var ctx = new TestContext(); // Initialize bUnit test context
        ctx.Services.AddSingleton(userManager);
        var testAuth = ctx.AddTestAuthorization();
        
        testAuth.SetAuthorized("testuser");

        var cut = ctx.RenderComponent<Home>();

        Assert.True(cut.Instance._isAuthenticated);
        

    }

    [Fact]
    public void Unauthenticated_User_Should_Set_IsAuthenticated_Local_Variable_To_False()
    {
        // Arrange
        using var ctx = new TestContext(); // Initialize bUnit test context
        ctx.Services.AddSingleton(userManager);
        var testAuth = ctx.AddTestAuthorization();
        
        testAuth.SetNotAuthorized();

        var cut = ctx.RenderComponent<Home>();

        Assert.False(cut.Instance._isAuthenticated);
    }

    [Fact]
    public void Authenticated_User_Without_Admin_Role_Should_Set_IsAdmin_Local_Variable_To_False()
    {
        // Arrange
        using var ctx = new TestContext(); // Initialize bUnit test context
        ctx.Services.AddSingleton(userManager);
        var testAuth = ctx.AddTestAuthorization();
        

        testAuth.SetAuthorized("testuser");
        

        var cut = ctx.RenderComponent<Home>();

        
        Assert.False(cut.Instance._isAdminRole);
    }

    [Fact]
    public void Authenticated_User_With_Admin_Role_Should_Set_IsAdmin_Local_Variable_To_True()
    {
        // Arrange
        using var ctx = new TestContext(); // Initialize bUnit test context
        ctx.Services.AddSingleton(userManager);
        var testAuth = ctx.AddTestAuthorization();


        testAuth.SetAuthorized("testuser");
        testAuth.SetRoles("Admin");

        var cut = ctx.RenderComponent<Home>();

        
        Assert.True(cut.Instance._isAdminRole);
    }

    public static class UserManagerHelper
    {
        public static UserManager<ApplicationUser> CreateUserManager()
        {
            var store = Substitute.For<IUserStore<ApplicationUser>>();
            var options = Substitute.For<IOptions<IdentityOptions>>();
            var passwordHasher = Substitute.For<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>> { Substitute.For<IUserValidator<ApplicationUser>>() };
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { Substitute.For<IPasswordValidator<ApplicationUser>>() };
            var keyNormalizer = Substitute.For<ILookupNormalizer>();
            var errors = Substitute.For<IdentityErrorDescriber>();
            var services = Substitute.For<IServiceProvider>();
            var logger = Substitute.For<ILogger<UserManager<ApplicationUser>>>();

            return new UserManager<ApplicationUser>(store, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);
        }
    }

    
}
