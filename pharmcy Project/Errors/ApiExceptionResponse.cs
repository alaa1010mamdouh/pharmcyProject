namespace pharmcy_Project.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details;
        }
        //public ApiExceptionResponse(int statusCode, string? message = null) : base(statusCode, message)
        //{
        //    Details = null;
        //}

    }
}
