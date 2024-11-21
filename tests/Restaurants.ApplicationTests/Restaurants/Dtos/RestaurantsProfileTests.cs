using Xunit;
using Restaurants.Application.Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

namespace Restaurants.Application.Restaurants.Dtos.Tests
{
    public class RestaurantsProfileTests
    {
        private IMapper _mapper;
        public RestaurantsProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();
            });

            _mapper = configuration.CreateMapper();
        }
        [Fact()]
    
        public void CreateMap_ForResraurantToRestaurantDto_MapsCorrectly()
        {
           
            // Arrange
            var restaurant = new Restaurant
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "Test description",
                Category = "Test category",
           
                Address = new Address
                {
                    City = "Test City",
                    Street = "Test Street",
                    PostalCode = "12-345"
                },
             
            };

            // Act
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            // Assert
            restaurantDto.Should().NotBeNull();
            restaurantDto.Id.Should().Be(restaurant.Id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            restaurantDto.Category.Should().Be(restaurant.Category);
        
            restaurantDto.City.Should().Be(restaurant.Address.City);
            restaurantDto.Street.Should().Be(restaurant.Address.Street);
            restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
     
        }


        [Fact()]

        public void CreateMap_ForCreateResraurantCommandToRestaurant_MapsCorrectly()
        {

            // Arrange
            var command = new CreateRestaurantCommand
            {
               
                Name = "Test Restaurant",
                Description = "Test description",
                Category = "Test category",
                ContactEmail = "test@example.com",
                ContactNamber = "+1234567890",
                City = "Test City",
                Street = "Test Street",
                PostalCode = "12-345"
              

            };

            // Act
            var restaurant   = _mapper.Map<Restaurant>(command);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.Category.Should().Be(command.Category);
            restaurant.ContactEmail.Should().Be(command.ContactEmail);
            restaurant.ContactNamber.Should().Be(command.ContactNamber);
            restaurant.Address.Should().NotBeNull();
            restaurant.Address.City.Should().Be(command.City);
            restaurant.Address.Street.Should().Be(command.Street);
            restaurant.Address.PostalCode.Should().Be(command.PostalCode);

        }


        [Fact]
        public void CreateMap_ForUpdateeResraurantCommandToRestaurant_MapsCorrectly()
        {
            // Arrange
            var updateCommand = new UpdateRestaurantCommand
            {
                Id = 1,
                Name = "Updated Restaurant",
                Description = "Updated description",
                HasDelivery = true,
            
            };

            // Act
            var restaurant = _mapper.Map<Restaurant>(updateCommand);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(updateCommand.Id);
            restaurant.Name.Should().Be(updateCommand.Name);
            restaurant.Description.Should().Be(updateCommand.Description);
            restaurant.HasDelivery.Should().Be(updateCommand.HasDelivery);
     
   
        }
    }
}