using pharmcy_Project.Errors;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pharmcy_Project.Middlewares
{
    public class ExceptionmiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionmiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionmiddleWare(RequestDelegate Next,ILogger<ExceptionmiddleWare>logger,IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }
        //invocAsync
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //if (_env.IsDevelopment())
                //{
                //    var response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message,ex.StackTrace.ToString());


                //}
                //else 
                //{ 
                //    var response = new ApiResponse((int)HttpStatusCode.InternalServerError);
                //}
                  var response = _env.IsDevelopment() 
                    ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                   
                    : new ApiResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                   
                };
                var jsonResponse=JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(jsonResponse);
            }
            

        }

    }
}
