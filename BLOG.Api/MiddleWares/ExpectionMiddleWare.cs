using BLOG.Api.Errors;
using System.Net;
using System.Text.Json;

namespace BLOG.Api.MiddleWares
{
    public class ExpectionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExpectionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExpectionMiddleWare(RequestDelegate Next, ILogger<ExpectionMiddleWare> logger, IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }
        //invoke
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment() ? new ApiExpectionResponse(httpContext.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) : new ApiExpectionResponse(httpContext.Response.StatusCode, "Internal Server Error");
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
               
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
