﻿
using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopWatch.Stop();
        if(stopWatch.ElapsedMilliseconds / 1000 > 4)
        {
            logger.LogInformation("Request [{verb}] at {path} took {Time} s",
                context.Request.Method,
                context.Request.Path,
                stopWatch.ElapsedMilliseconds /1000
                );
        }
    }
}
