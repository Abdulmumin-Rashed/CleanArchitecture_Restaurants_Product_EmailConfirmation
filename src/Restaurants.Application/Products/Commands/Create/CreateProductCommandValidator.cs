using FluentValidation;
using MediatR;
using Restaurants.Domain.Repositories;
using System.Threading;

namespace Restaurants.Application.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateProductCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Product.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Product.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be a valid ID.")
                .Must(CategoryExists).WithMessage(x => $"Category with this ID {x.Product.CategoryId} does not exist.");

        }

        private bool CategoryExists(int categoryId)
        {
            // Call the synchronous version of ExistsAsync
            return _categoryRepository.Exists(categoryId);
        }
    }
}