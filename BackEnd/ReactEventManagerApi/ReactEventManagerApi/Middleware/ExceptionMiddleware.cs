
using Application.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ReactEventManagerApi.Middleware
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment environment) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex) { await HandleValidationException(context, ex); }
            catch (Exception ex)
            {

                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = environment.IsDevelopment()
                ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new AppException(context.Response.StatusCode, ex.Message, null);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }

        private static async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            var valdiationErrors = new Dictionary<string, string[]>();
            if (ex.Errors is not null)
            {
                foreach (var error in ex.Errors)
                {
                    if (valdiationErrors.TryGetValue(error.PropertyName, out var existingErrors))
                    {
                        valdiationErrors[error.PropertyName] = [.. existingErrors, error.ErrorMessage]; // collection expression is ..
                        //valdiationErrors[error.PropertyName] = existingErrors.Append(error.ErrorMessage).ToArray(); // same as above line 
                    }
                    else
                    {
                        valdiationErrors[error.PropertyName] = [error.ErrorMessage];
                    }
                }
            }
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var validationProblemDetails = new ValidationProblemDetails(valdiationErrors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValdiationFailure",
                Title = "Valdiation Error",
                Detail = "One or more valdiation errors occured"
            };
            await context.Response.WriteAsJsonAsync(validationProblemDetails);
        }
    }
}
