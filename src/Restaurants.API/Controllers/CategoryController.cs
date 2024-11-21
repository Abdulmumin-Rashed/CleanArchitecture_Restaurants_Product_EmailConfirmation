using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Categories.Commands.CreateCategory;
using Restaurants.Application.Categories.Dtos;
using Restaurants.Application.Categories.Queries.GetCategoryById;
using Restaurants.Domain.Constants;


namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        [HttpGet("{id}")]
        [Authorize(Roles =UserRoles.Admin)]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var category = await _mediator.Send(query);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
        {
            var categoryId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, categoryId);
        }
    }
}
