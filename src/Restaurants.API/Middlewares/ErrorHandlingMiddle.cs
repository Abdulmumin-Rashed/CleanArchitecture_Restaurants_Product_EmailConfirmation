
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares
{
    public class ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFound)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound; 
                await context.Response.WriteAsync(notFound.Message);
                logger.LogInformation(notFound.Message);
            }
            catch (ForbidException ex)
            {
                logger.LogWarning("Forbidden access: {Message}", ex.Message);
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Access Forbidden\"}");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("SomeThing Went Wrong");
            }
        }
    }
}
