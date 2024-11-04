namespace RuynServer.MIddleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    public class AuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {

        // TODO: Move to a less stupid place
        private const string VERSION = "V0.1.6";

        private readonly RequestDelegate _next = next;
        private const string ApiKeyHeaderName = "X-Api-Key"; // Custom header name
        private readonly string? _apiKey = configuration.GetValue<string>("ApiKeySettings:Key");

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync($"{VERSION}");
                return;
            }

            if (_apiKey is not null && !_apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await _next(context); 
        }
    }
}
