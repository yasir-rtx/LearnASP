// using FluentValidation;
using LearnASP.Application.Common.Exceptions;
using LearnASP.Application.Common.Responses;

namespace LearnASP.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            // Validation Exception
            // catch (ValidationException ex)
            // {
            //     context.Response.StatusCode = StatusCodes.Status400BadRequest;

            //     await context.Response.WriteAsJsonAsync(
            //         ApiResponse<object>.ErrorResponse(
            //             "Validation failed",
            //             ex.Errors
            //         )
            //     );
            // }

            // Not Found Exception
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse(ex.Message)
                );
            }

            // Domain Exception
            catch (DomainException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse(ex.Message)
                );
            }

            // Others
            catch (Exception ex)
            {
                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                var message = _env.IsDevelopment()
                    ? ex.Message
                    : "Internal server error";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse(message)
                );
            }
        }
    }
}
