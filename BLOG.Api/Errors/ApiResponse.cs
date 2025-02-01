using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BLOG.Api.Errors
{
    public class ApiResponse
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int? statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message?? GetDefaultMessageForStatusCode(StatusCode);
        }
        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "A bad request, you have made",
                401 => " You are not Authorized",
                404 => "Resource not found ",
                500 => "Internal Serval Error",
                _ => null
            };
        }
    }
}
