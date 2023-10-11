using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using paf.api.Services;
using System.Text.Json;

namespace paf.api.Middleware
{
    public class ExceptionMiddleWare
    {
        private RequestDelegate next;
        public ExceptionMiddleWare(RequestDelegate Next)
        {
            next = Next;
        }
        public async Task Invoke(HttpContext Context)
        {
            try
            {
                await next(Context);
            }
            catch (NotFoundException ex)
            {
                Context.Response.StatusCode = StatusCodes.Status400BadRequest;
                Context.Response.ContentType = "application/problem+json";
                var problemDetails = new ProblemDetails
                {
                    Type = "Error",
                    Status = StatusCodes.Status400BadRequest,
                    Title = "not found error",
                    Detail = ex.Message,
                    Instance = "",
                };
                var problemInJson = JsonSerializer.Serialize(problemDetails);
                await Context.Response.WriteAsync(problemInJson);
            }
            catch (ValidationException ex)
            {
                Context.Response.StatusCode = StatusCodes.Status400BadRequest;
                Context.Response.ContentType = "application/problem+json";
                var problemDetails = new ProblemDetails
                {
                    Type = "Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "validation error ! try again later",
                    Detail = ex.Message,
                    Instance = "",
                };
                var problemInJson = JsonSerializer.Serialize(problemDetails);
                await Context.Response.WriteAsync(problemInJson);
            }
            catch (Exception ex)
            {
                Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                Context.Response.ContentType = "application/problem+json";
                var problemDetails = new ProblemDetails
                {
                    Type = "Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "intenal server error ! something went wrong",
                    Detail= ex.Message,
                    Instance="",
                };
                var problemInJson=JsonSerializer.Serialize(problemDetails);
                 await Context.Response.WriteAsync(problemInJson);
            }
        }
    }
}
