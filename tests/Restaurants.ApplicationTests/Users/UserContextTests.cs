using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System.Security.Claims;
using Xunit;


namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticationUser_ShouldReturnCurrentUser()
        {
            //  arrange
            var dateOfBirth = new DateOnly(1999, 1, 1);
            var httpContextAccessMock = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim>()
            {
                    new(ClaimTypes.NameIdentifier,"1"),
                    new(ClaimTypes.Email,"test@test.com"),
                    new(ClaimTypes.Role,UserRoles.Admin),
                    new(ClaimTypes.Role,UserRoles.User),
                    new("Nationality","Yemeni"),
                    new("DateOfBirth",dateOfBirth.ToString("yyyy-MM-dd"))
            };
        
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
            httpContextAccessMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext() 
            {
                User = user
            
            });
            var userContext = new UserContext(httpContextAccessMock.Object);

            // act

            var currentUser = userContext.GetCurrentUser();
            // assert

            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.Nationality.Should().Be("Yemeni");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);


        }


        [Fact()]
        public void GetCurrentUser_WithUserContextNoPresent_ThrowInvalidOperationException()
        {
            //  arrange
            var httpContextAccessMock = new Mock<IHttpContextAccessor>();
            httpContextAccessMock.Setup(x => x.HttpContext).Returns( (HttpContext)null);
          
            var userContext = new UserContext(httpContextAccessMock.Object);

            // act
            Action action = () => userContext.GetCurrentUser();



            // assert

            action.Should().Throw<InvalidOperationException>()
                .WithMessage("User context is not Present");


        }

    }




}