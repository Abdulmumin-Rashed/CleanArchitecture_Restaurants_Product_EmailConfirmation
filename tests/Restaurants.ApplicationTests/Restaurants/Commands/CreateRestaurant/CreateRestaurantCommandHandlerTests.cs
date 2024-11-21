using Xunit;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Application.Users;
using FluentAssertions;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
        {
            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
            var mapperMock = new Mock<IMapper>();
            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();
            mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
            var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantsRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Restaurant>()))
                .ReturnsAsync(1);
            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("owner-id", "test@test.com", [], null, null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object,
                mapperMock.Object, userContextMock.Object,
                restaurantsRepositoryMock.Object
               );

            // act

            var result = await commandHandler.Handle(command,CancellationToken.None);

            // assert

            result.Should().Be(1);
            restaurantsRepositoryMock.Verify( r => r.Create(restaurant),Times.Once);


        }
    }
}