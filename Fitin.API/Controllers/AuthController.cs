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
    private readonly IWebHostEnvironment _environment;

    public AuthController(AuthService authService, IUserRepository userRepository, IWebHostEnvironment environment)
    {
        _authService = authService;
        _userRepository = userRepository;
        _environment = environment;
    }

    private CookieOptions BuildAuthCookieOptions(DateTime expiresUtc)
    {
        var isDevelopment = _environment.IsDevelopment();
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !isDevelopment,
            SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = expiresUtc
        };
    }

    private void SetAuthCookies(AuthResponseDto authResponse)
    {
        Response.Cookies.Append("accessToken", authResponse.AccessToken, BuildAuthCookieOptions(authResponse.AccessTokenExpiresAt));
        Response.Cookies.Append("refreshToken", authResponse.RefreshToken, BuildAuthCookieOptions(authResponse.RefreshTokenExpiresAt));
    }

    private void ClearAuthCookies()
    {
        var expiredAt = DateTime.UtcNow.AddDays(-1);

        Response.Cookies.Append("accessToken", "", BuildAuthCookieOptions(expiredAt));
        Response.Cookies.Append("refreshToken", "", BuildAuthCookieOptions(expiredAt));
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

        SetAuthCookies(result);

        return Success(result, "Login successful");
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
            return Failure("Refresh token is missing", statusCode: 401);

        var result = await _authService.RefreshAsync(refreshToken);
        SetAuthCookies(result);

        return Success(result, "Token refreshed successfully");
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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync(Request.Cookies["refreshToken"]);
        ClearAuthCookies();

        return Success<object?>(null, "Logged out successfully");
    }
}
