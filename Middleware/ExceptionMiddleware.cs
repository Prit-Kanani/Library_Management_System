using FluentValidation;
using Library_Management_System.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Library_Management_System.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        #region Pipeline

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        #endregion

        #region Handling

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            if (ex is ValidationException validationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var validationResponse = new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Validation failed",
                    Errors = validationException.Errors
                        .GroupBy(error => error.PropertyName)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Select(error => error.ErrorMessage).ToArray()),
                    Status = "ERROR"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(validationResponse));
                return;
            }

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "The API is not working";

            if (ex is ApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = apiException.Message;
            }

            context.Response.StatusCode = statusCode;

            var errorResponse = new
            {
                StatusCode = statusCode,
                Message = message,
                Status = "ERROR"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        #endregion
    }
}
