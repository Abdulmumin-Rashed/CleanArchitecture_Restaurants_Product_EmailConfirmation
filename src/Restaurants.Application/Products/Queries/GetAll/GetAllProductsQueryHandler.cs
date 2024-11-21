

using AutoMapper;
using MediatR;
using Restaurants.Application.Products.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Products.Queries.GetAll;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync();
        var productsDto = _mapper.Map<List<ProductDto>>(products);

        return productsDto;
    }
}
