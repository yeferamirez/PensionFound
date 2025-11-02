using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PensionFund.Application.Context;

namespace PensionFund.Api.Middleware;

public class ExceptionsLoggerMiddleware(
    RequestDelegate next,
    ILogger<ExceptionsLoggerMiddleware> logger,
    CurrentEnvironment environment)
{
    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            context.Request.EnableBuffering();
            await next(context);
        }
        catch (Exception ex)
        {
            var url = context.Request.GetDisplayUrl();
            logger.LogError(ex, "Error executing {Url}", url);
            var problemDetails = new ProblemDetails
            {
                Title = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
                Type = url
            };
            if (environment.IsProduction())
            {
                problemDetails.Detail = "An error occurred while processing your request. Please try again later.";
            }
            else
            {
                problemDetails.Detail = ex.ToString();
            }
            var jsonString = JsonConvert.SerializeObject(problemDetails);
            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonString, System.Text.Encoding.UTF8);
        }
    }
}

