using Fitin.Application.Authentication.Interfaces;
using Fitin.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Fitin.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseApiController
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
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
    public async Task<IActionResult> Login(LoginRequestDto dto)
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
    public IActionResult Profile()
    {
        return Success("You are authenticated", "Profile fetched successfully");
    }
}
