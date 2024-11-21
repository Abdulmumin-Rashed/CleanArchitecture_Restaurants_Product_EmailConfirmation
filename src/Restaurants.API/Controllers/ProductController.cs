
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Products.Commands.Create;
using Restaurants.Application.Products.Dtos;
using Restaurants.Application.Products.Queries.GetAll;
using Restaurants.Application.Products.Queries.GetById;

namespace Restaurants.API.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);
        if (product == null) return NotFound();
        return Ok(product);
    }
    [HttpGet(Name = "GetAll")]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var query = new GetAllProductsQuery(); // Ensure you have a GetAllProductsQuery defined
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        try
        {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = productId }, productId);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception (not shown here)
            return StatusCode(500, "An unexpected error occurred.");
        }
    }



}
}
