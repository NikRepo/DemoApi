using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit.Sdk;
using DemoApi.Model;

namespace DemoApi.Middleware
{
    /// <summary>
    /// This is middleware to handle the error - e.g. for valid json or server side error
    /// </summary>
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddleware> _logger;
        public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;   
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                #region Validate Json Payload - Read and check
                if (httpContext.Request.ContentType != null && httpContext.Request.ContentType.Contains("application/json"))
                {
                    httpContext.Request.EnableBuffering();

                    using (var reader = new StreamReader(httpContext.Request.Body, leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        if (string.IsNullOrWhiteSpace(body))
                        {
                            _logger.LogError("Request Body is Empty");
                            await HandleInvalidJson(httpContext, "Request Body is Empty");
                            return;
                        }

                        try
                        {
                            JsonDocument.Parse(body);
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError("Invalid JSON payload.");
                            await HandleInvalidJson(httpContext, ex.Message);
                            return;
                        }

                        // Reset the request body stream position
                        httpContext.Request.Body.Position = 0;
                    }
                }

                #endregion

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await  Handle500Exception(httpContext, ex);
            }
        }

        private Task Handle500Exception(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiErrorMessage()
            {
                Error = "An unexpected error occurred.",
                Details = ex.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return httpContext.Response.WriteAsync(jsonResponse);
        }
        private Task HandleInvalidJson(HttpContext context, string addDetails = "")
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
          
            var response = new ApiErrorMessage()
            {
                Error = "Invalid JSON payload",
                Details = addDetails
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
          
            var response = new ApiErrorMessage()
            {
                Error = "An error occurred while processing your request",
                Details = ""
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}
