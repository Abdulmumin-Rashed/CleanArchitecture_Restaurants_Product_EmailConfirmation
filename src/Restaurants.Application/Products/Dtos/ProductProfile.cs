

using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Products.Dtos;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();

    }
}