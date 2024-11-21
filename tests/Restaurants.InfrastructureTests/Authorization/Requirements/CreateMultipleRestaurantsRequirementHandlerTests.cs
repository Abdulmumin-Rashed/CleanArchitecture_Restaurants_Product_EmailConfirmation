using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;
using FluentAssertions;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class CreateMultipleRestaurantsRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreateMultipleRestaurants_ShouldSucceed()
        {
            //arrange
            var cuurentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(m => m.GetCurrentUser()).Returns(cuurentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = cuurentUser.Id,
                },
                new()
                {
                    OwnerId = cuurentUser.Id,
                } ,
                new()
                {
                    OwnerId = "2",
                }
                
            };
            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirment = new CreatedMultipleRestaurantsRequirement(2);
            var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object,
                userContextMock.Object);

            var context = new AuthorizationHandlerContext([requirment], null, null);

            //act

            await handler.HandleAsync(context);

            //assert

            context.HasSucceeded.Should().BeTrue();


        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreateMultipleRestaurants_ShouldFaild()
        {
            //arrange
            var cuurentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(m => m.GetCurrentUser()).Returns(cuurentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = cuurentUser.Id,
                },
             
                new()
                {
                    OwnerId = "2",
                }

            };
            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirment = new CreatedMultipleRestaurantsRequirement(2);
            var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object,
                userContextMock.Object);

            var context = new AuthorizationHandlerContext([requirment], null, null);

            //act

            await handler.HandleAsync(context);

            //assert

            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();


        }
    }
}