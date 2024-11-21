using MediatR;
using Restaurants.Application.Products.Dtos;

namespace Restaurants.Application.Products.Queries.GetById
{
    public class GetProductByIdQuery(int id) : IRequest<ProductDto>
    {
        public int Id { get; set; } = id;
    }
}
