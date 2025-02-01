namespace BLOG.Api.Errors
{
    public class ApiExpectionResponse:ApiResponse
    {
        public ApiExpectionResponse(int StatusCode, string? Message = null, string?details = null) : base(StatusCode, Message)
        {
            Details = details;
        }
        public string? Details { get; set; }
    }
}
