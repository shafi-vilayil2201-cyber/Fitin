using Fitin.Application.Authentication.Interfaces;
using Fitin.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Fitin.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseApiController
{
    private readonly AuthService _authService;
    private readonly IUserRepository _userRepository;

    public AuthController(AuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterRequestDto dto)
    {
        // var result = await _authService.RegisterAsync(dto);

        // Response.Cookies.Append("accessToken", result.AccessToken, new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = false,
        //     SameSite = SameSiteMode.Strict,
        //     Expires = DateTime.UtcNow.AddMinutes(15)
        // });

        // Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = false,
        //     SameSite = SameSiteMode.Strict,
        //     Expires = DateTime.UtcNow.AddDays(1)
        // });

        // return Success(result, "Register successful");

        await _authService.RegisterAsync(dto);
        return Success<Object?>(null,"Register successfull. Please login");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        Response.Cookies.Append("accessToken", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });

        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(1)
        });

        return Success(result, "Login successful");
    }
 
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Failure("Invalid token", statusCode: 401);

        var dbUser = await _userRepository.GetByIdAsync(userId);
        if (dbUser == null)
            return Failure("User not found", statusCode: 404);

        var user = new
        {
            Id       = dbUser.Id,
            Name     = dbUser.Name,
            Email    = dbUser.Email,
            Role     = dbUser.Role.ToString(),
            IsActive = dbUser.IsActive
        };
        return Success(user, "Profile fetched successfully");
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure   = false,
            SameSite = SameSiteMode.Strict,
            Expires  = DateTime.UtcNow.AddDays(-1) // expire immediately
        };

        Response.Cookies.Append("accessToken",  "", cookieOptions);
        Response.Cookies.Append("refreshToken", "", cookieOptions);

        return Success<object?>(null, "Logged out successfully");
    }
}
