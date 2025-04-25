using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;
using System.Text.Json;
using FormAdvanced.BuildingBlocks.Domain.Exceptions;

using ApplicationException = FormAdvanced.BuildingBlocks.Domain.Exceptions.ApplicationException;

namespace FormAdvanced.API.Middleware
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new
            {
                title = GetTitle(exception),
                status = statusCode,
                detail = exception.Message,
                errors = exception is ValidationException validationException ? validationException.ErrorsDictionary : GetGeneralErrors(exception)
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                AuthenticationException => StatusCodes.Status401Unauthorized,
				SecurityTokenException => StatusCodes.Status401Unauthorized,
				ValidationException => StatusCodes.Status422UnprocessableEntity,
                ConflictException => StatusCodes.Status409Conflict,
				_ => StatusCodes.Status500InternalServerError
            };
        private static string GetTitle(Exception exception) =>
            exception switch
            {
                ApplicationException applicationException => applicationException.Title,
                _ => "Server Error"
            };

		private static IReadOnlyDictionary<string, string[]> GetGeneralErrors(Exception exception)
		{
			return new Dictionary<string, string[]>
	        {
		        { "Error", new[] { exception.Message } }
	        };
		}
	}
}
