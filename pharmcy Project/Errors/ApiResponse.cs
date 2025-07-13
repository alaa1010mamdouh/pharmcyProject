
namespace pharmcy_Project.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse( int statuecode,string? message=null)
        {
            StatusCode = statuecode;
            Message = message??GetDefaultMessage(StatusCode);
            
        }

        private string? GetDefaultMessage(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, was not",
                500 => "An error occurred on the server",
                _ => null

            };
        }
    }
}
