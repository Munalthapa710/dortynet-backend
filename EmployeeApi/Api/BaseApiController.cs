using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EmployeeApi.Api
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult SuccessResponse(string message, object? data = null)
        {
            return Ok(new ApiResponse<object?>
            {
                Code = StatusCodes.Status200OK,
                Message = message,
                Data = data,
                Errors = Array.Empty<string>()
            });
        }

        protected IActionResult CreatedResponse(string message, object? data = null)
        {
            return Ok(new ApiResponse<object?>
            {
                Code = StatusCodes.Status201Created,
                Message = message,
                Data = data,
                Errors = Array.Empty<string>()
            });
        }

        protected IActionResult ErrorResponse(int code, string message)
        {
            return Ok(new ApiResponse<object?>
            {
                Code = code,
                Message = message,
                Data = null,
                Errors = new[] { message }
            });
        }

        protected IActionResult ValidationResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToArray();

            return Ok(new ApiResponse<object?>
            {
                Code = 600,
                Message = "Validation Failed",
                Data = null,
                Errors = errors
            });
        }

        protected IActionResult NotFoundResponse(string message = "Data not found.")
        {
            return Ok(new ApiResponse<object?>
            {
                Code = StatusCodes.Status404NotFound,
                Message = message,
                Data = null,
                Errors = new[] { message }
            });
        }

        protected IActionResult ExceptionResponse(Exception exception)
        {
            return Ok(new ApiResponse<object?>
            {
                Code = StatusCodes.Status500InternalServerError,
                Message = "Something went wrong.",
                Data = null,
                Errors = new[] { exception.Message }
            });
        }
    }
}