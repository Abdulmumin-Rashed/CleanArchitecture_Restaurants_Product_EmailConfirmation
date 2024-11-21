
using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 20, 25, 30,35,40];
    private string[] allowedSortByColumnNames = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor( r => r.PageNumber )
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowPageSizes)}]");

             RuleFor(r => r.SortBy)
            .Must(value => allowedSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");

    }
}
