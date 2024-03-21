using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Showcase.Application.Common.Interfaces.Services;

namespace Showcase.Api.Controllers;

public class ApiController : ControllerBase
{
    protected readonly IJwtTokenService _tokenService;

    protected ApiController(IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected async Task<string> GetUserIdFromTokenAsync()
    {
        // Get the Authorization header
        string authHeader = Request.Headers["Authorization"];

        // Split the string to extract the JWT part (ignoring "Bearer ")
        string[] parts = authHeader.Split(' ');

        if (parts.Length != 2)
        {
            // Handle error: Invalid format
            throw new ArgumentException("Invalid JWT format. Expected 'Bearer <token>'");
        }

        string jwtToken = parts[1]; // Second element is the JWT token

        // Decode the UserId from the token
        return await _tokenService.DecodeUserIdFromToken(jwtToken);
    }
    
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        Error firstError = errors[0];
        return Problem(firstError);
    }

    private IActionResult Problem(Error error)
    {
        int statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        ModelStateDictionary modelStateDictionary = new();
        foreach (Error error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}