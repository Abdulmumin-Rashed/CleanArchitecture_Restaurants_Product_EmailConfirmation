using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Restaurants.Application.Restaurants.Queries.CheckEmailExistence;
using Xunit;


namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandValidatorTests
    {

        [Fact]
        public void  Validator_ForValidCommand_ShouldNotHaveValidationError()
        {
            var command = new CreateRestaurantCommand
            {
                Name = "Valid Restaurant Name",
                Description = "A nice place to eat",
                Category = "Italian",
                ContactEmail = "existing.email@example.com", // Email that already exists
                ContactNamber = "+1234567890",
                PostalCode = "12-345"
            };

            var validator = new CreateRestaurantCommandValidator();
            
            //act
            
            var result = validator.TestValidate(command);

            // assert

            result.ShouldNotHaveAnyValidationErrors();

        }


        [Fact]
        public void  Validator_ForInValidCommand_ShouldNotHaveValidationError()
        {
            var command = new CreateRestaurantCommand
            {
                Name = "Va",
                Description = "",
                Category = "",
                ContactEmail = "@test.com", // Email that already exists
                ContactNamber = "gfg",
                PostalCode = "12345"
            };

            var validator = new CreateRestaurantCommandValidator();

            //act

            var result = validator.TestValidate(command);

            // assert

            result.ShouldHaveValidationErrorFor( c => c.Name);
            result.ShouldHaveValidationErrorFor( c => c.Category);
            result.ShouldHaveValidationErrorFor( c => c.ContactEmail);
            result.ShouldHaveValidationErrorFor( c => c.ContactNamber);
            result.ShouldHaveValidationErrorFor( c => c.PostalCode);

        }

        [Theory()]
        [InlineData("Italian")]
        [InlineData("Mexican")]
        [InlineData("Japanese")]
        [InlineData("American")]
        [InlineData("Indian")]
        public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand { Category = category };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveValidationErrorFor(c => c.Category);
        }


        [Theory()]
        [InlineData("10220")]
        [InlineData("102-20")]
        [InlineData("10 200")]
        [InlineData("10-2 55")]
        [InlineData("88-8855")]
        public void Validator_ForInValidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand { PostalCode = postalCode };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

    }
}