using DemoApi.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoApi.Middleware
{
    public class MissingDataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddleware> _logger;
        public MissingDataMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if ((httpContext.Request.Method == HttpMethods.Post || httpContext.Request.Method == HttpMethods.Put) &&
            httpContext.Request.ContentType != null && httpContext.Request.ContentType.Contains("application/json"))
            {
                httpContext.Request.EnableBuffering();

                using (var reader = new StreamReader(httpContext.Request.Body, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    JObject Ob = JObject.Parse(body);
                    if(Ob.TryGetValue("name", StringComparison.OrdinalIgnoreCase , out JToken? jToken) == false)
                    {
                        await HandleMissingData(httpContext, "Missing required field - name");
                    }
                }

                // Reset the request body stream position
                httpContext.Request.Body.Position = 0;
            }

            await _next(httpContext);
        }

        private Task HandleMissingData(HttpContext context, string addDetails)
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
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MissingDataMiddlewareExtensions
    {
        public static IApplicationBuilder UseMissingDataMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MissingDataMiddleware>();
        }
    }
}
