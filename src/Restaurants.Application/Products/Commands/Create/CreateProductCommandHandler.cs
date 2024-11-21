using AutoMapper;
using MediatR;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Products.Commands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if the category exists
            //var categoryExists = await _categoryRepository.ExistsAsync(request.Product.CategoryId);
            //if (!categoryExists)
            //{
            //    throw new ValidationException($"Category with ID {request.Product.CategoryId} does not exist.");
            //}

            // Map the request to the product entity
            var product = _mapper.Map<Product>(request.Product);
            await _productRepository.AddAsync(product);
            return product.Id; // Save changes to DB if needed
        }
    }
}