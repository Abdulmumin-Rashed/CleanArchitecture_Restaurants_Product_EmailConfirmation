using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using FluentAssertions;
using Restaurants.Domain.Exceptions;
using Xunit;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateRestaurantCommandHandler _handler;
        private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationService;

        public UpdateRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
            _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
            _mapperMock = new Mock<IMapper>();
            _restaurantAuthorizationService = new Mock<IRestaurantAuthorizationService>();
            _handler = new UpdateRestaurantCommandHandler(
                _loggerMock.Object,
                _restaurantsRepositoryMock.Object,
                _mapperMock.Object,
                _restaurantAuthorizationService.Object
            );
        }

        [Fact]
        public async Task Handle_ForValidCommand_UpdatesRestaurant()
        {
            // Arrange
            var updateCommand = new UpdateRestaurantCommand
            {
                Id = 1,
                Name = "Updated Restaurant",
                Description = "Updated description",
                HasDelivery = true
            };

            var existingRestaurant = new Restaurant
            {
                Id = updateCommand.Id,
                Name = "Old Restaurant",
                Description = "Old description"
            };

            _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(updateCommand.Id))
                .ReturnsAsync(existingRestaurant);

            _mapperMock.Setup(m => m.Map(updateCommand, existingRestaurant))
                .Verifiable();

            _restaurantsRepositoryMock.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            _restaurantAuthorizationService.Setup(s => s.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update))
                .Returns(true);

            // Act
            await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            existingRestaurant.Name.Should().Be(updateCommand.Name);
            existingRestaurant.Description.Should().Be(updateCommand.Description);
            existingRestaurant.HasDelivery.Should().Be(updateCommand.HasDelivery);

            _mapperMock.Verify(m => m.Map(updateCommand, existingRestaurant), Times.Once);
            _restaurantsRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);

            // Verify logger was called with expected string
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Updating Restaurant with id :")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }



        [Fact]
        public async Task Handle_WhenRestaurantNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var restaurantId = 2;
            var updateCommand = new UpdateRestaurantCommand
            {
                Id = restaurantId,
            };

            // Set up the repository to return null for the restaurant
            _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Restaurant with id: {restaurantId} does not exist");
        }

        [Fact]
        public async Task Handle_WhenUnauthorized_ThrowsForbidException()
        {
            // Arrange
            var updateCommand = new UpdateRestaurantCommand
            {
                Id = 1,
                Name = "Updated Restaurant",
                Description = "Updated description",
                HasDelivery = true
            };

            var existingRestaurant = new Restaurant
            {
                Id = updateCommand.Id,
                Name = "Old Restaurant",
                Description = "Old description"
            };

            // Mock repository to return an existing restaurant
            _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(updateCommand.Id)).ReturnsAsync(existingRestaurant);

            // Mock the authorization service to fail the authorization
            _restaurantAuthorizationService.Setup(s => s.Authorize(existingRestaurant, ResourceOperation.Update)).Returns(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ForbidException>()
                .WithMessage("This Permission access is denied");
        }

    }
}
