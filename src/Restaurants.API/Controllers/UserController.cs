using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Auth.Commands.Email;
using Restaurants.Application.Auth.Commands.Login;
using Restaurants.Application.Auth.Commands.Register;
using Restaurants.Application.Auth.Commands.Token;
using Restaurants.Domain.HelperModels;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await _mediator.Send(new ConfirmEmailCommand
            {
                Token = token,
                Email = email
            });

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result); // Returns detailed errors if confirmation fails
        }

        [HttpPost("Refresh-Token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(AccessToken tokens)
        {
            if (string.IsNullOrEmpty(tokens.Token) || string.IsNullOrEmpty(tokens.RefreshToken))
            {
                return BadRequest("Token or Refresh Token cannot be null or empty.");
            }
            var command = new RefreshAccessTokenCommand(tokens.Token, tokens.RefreshToken); var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        //[HttpPost("register")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    if (!result.Success)
        //    {
        //        return BadRequest(result); // Returns error response if registration fails
        //    }

        //    // Generate the confirmation email link
        //    var confirmationLink = Url.Action(
        //        nameof(ConfirmEmail),
        //        "Authentication",
        //        new { token = result.Data, email = command.Email },
        //        protocol: HttpContext.Request.Scheme
        //    );

        //    // Send the email
        //    var message = new Message(
        //        recipients: new[] { command.Email },
        //        subject: "Confirm your email",
        //        content: $"<p>Please confirm your email by clicking the link below:</p><p><a href=\"{confirmationLink}\" style=\"color: blue;\">Click here to confirm</a></p>"
        //    );

        //    await _mediator.Send(new SendEmailCommand(message)); // Sends email via a mediator command for consistency

        //    return Ok(new Response(true, $"User registered successfully. Confirmation email sent to {command.Email}."));
        //}
        //[HttpGet("confirm-email")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{
        //    var result = await _mediator.Send(new ConfirmEmailCommand
        //    {
        //        Token = token,
        //        Email = email
        //    });

        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }

        //    return BadRequest(result); // Returns detailed errors if confirmation fails
        //}


        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    if (!result.Success)
        //    {
        //        return BadRequest(result);
        //    }
        //    return Ok(result);
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}

