

using MediatR;

namespace Restaurants.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = default!;
}
