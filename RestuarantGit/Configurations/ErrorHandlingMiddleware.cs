using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Delivery.Resutruant.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Allow the pipeline to continue
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.", ex);
            }
        }


        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, Exception ex = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                status = statusCode.ToString(),
                message,
                detailed = ex?.Message
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
