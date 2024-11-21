using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    [Authorize]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
       [AllowAnonymous]
        //  [ProducesResponseType(StatusCodes.Status200OK,Type= typeof(IEnumerable<RestaurantDto>))]
       // [Authorize(Policy = PolicyNames.CreatedAtLeast2Restaurants)]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
        {
            var restaurants = await mediator.Send(query);
            return Ok(restaurants);
        }
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
        {
             command.Id = id;
             await mediator.Send(command);
           
                return NoContent();


           
        }


        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.HasNationality,Roles =UserRoles.Admin)]
        public async Task<ActionResult<IEnumerable<RestaurantDto?>>> GetById([FromRoute] int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
     
            return Ok(restaurant);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
         
                return NoContent();


           
        }

        [HttpPost]
      //  [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command )
        {
            //if (!ModelState.IsValid)
            //{
            //    return ValidationProblem();
            //}
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById),  new {id},null);
        }
    }
}
