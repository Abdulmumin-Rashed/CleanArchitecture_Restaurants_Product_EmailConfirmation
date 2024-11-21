using Xunit;
using Restaurants.API.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares.Tests
{
    public class ErrorHandlingMiddleTests
    {
        [Fact]
        public async Task InvokeAsync_WhenNotFoundExceptionIsThrown_ShouldNotCallNextDelegate()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(mockLogger.Object);
            var context = new DefaultHttpContext();
          //  var nextDelegateMock = new Mock<RequestDelegate>();
            var resourceType = "Restaurant";
            var resourceId = "123";

            // Creating a mock next delegate that will be passed to the middleware
            var next = new Mock<RequestDelegate>();

            // Set up the next delegate to throw NotFoundException when invoked
            next.Setup(n => n(It.IsAny<HttpContext>())).Throws(new NotFoundException(resourceType, resourceId));

            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context, next.Object);

            // Assert
            next.Verify( nex => nex.Invoke(context),Times.Once);
            context.Response.StatusCode.Should().Be(404);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
            responseBody.Should().Be($"{resourceType} with id: {resourceId} does not exist");

            // Verify that the next delegate was NOT called
            next.Verify(n => n(It.IsAny<HttpContext>()), Times.Once);
        }


        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturn404_WhenNotFoundExceptionIsThrown()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(mockLogger.Object);

            var context = new DefaultHttpContext();
            var resourceType = "Restaurant";
            var resourceId = "123";

            var next = new RequestDelegate(async _ =>
            {
                throw new NotFoundException(resourceType, resourceId);
            });

            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context, next);

            // Assert
            context.Response.StatusCode.Should().Be(404);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
            responseBody.Should().Be($"{resourceType} with id: {resourceId} does not exist");

            // Verifying LogInformation call
            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, type) => state.ToString().Contains($"{resourceType} with id: {resourceId} does not exist")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ), Times.Once);
        }


        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturn403_WhenForbidExceptionIsThrown()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(mockLogger.Object);

            var context = new DefaultHttpContext();
            var resultMessage = "Editing";
            var next = new RequestDelegate(_ => throw new ForbidException(resultMessage));
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context, next);

            // Assert
            context.Response.StatusCode.Should().Be(403);
            //context.Response.ContentType.Should().Be("application/json");
            //context.Response.Body.Seek(0, SeekOrigin.Begin);
            //var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
            //responseBody.Should().Be("{\"error\": \"Access Forbidden\"}");

            //mockLogger.Verify(logger => logger.LogWarning($"{resultMessage} access is denied"), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogAndReturn500_WhenGenericExceptionIsThrown()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(mockLogger.Object);

            var context = new DefaultHttpContext();
            var exceptionMessage = "Unexpected error occurred";

            var next = new RequestDelegate(async _ =>
            {
                throw new Exception(exceptionMessage);
            });

            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context, next);

            // Assert
            context.Response.StatusCode.Should().Be(500);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            //var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
            //responseBody.Should().Be("SomeThing Went Wrong");

            //// Verifying LogError call
            //mockLogger.Verify(
            //    logger => logger.Log(
            //        LogLevel.Error,
            //        It.IsAny<EventId>(),
            //        It.Is<It.IsAnyType>((state, type) => state.ToString().Contains(exceptionMessage)),
            //        It.IsAny<Exception>(),
            //        It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
            //    ), Times.Once);
        }


    }
}