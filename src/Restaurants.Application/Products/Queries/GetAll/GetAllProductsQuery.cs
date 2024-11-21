using MediatR;
using Restaurants.Application.Products.Dtos;

namespace Restaurants.Application.Products.Queries.GetAll
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>> { }
}
