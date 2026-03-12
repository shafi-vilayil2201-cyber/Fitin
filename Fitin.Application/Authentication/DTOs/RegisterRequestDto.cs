using System.ComponentModel.DataAnnotations;

namespace Fitin.Application.DTOs;

public record RegisterRequestDto(
    [Required]
    [StringLength(100, MinimumLength = 2)]
    string Name,
    [Required]
    [EmailAddress]
    [StringLength(256)]
    string Email,
    [Required]
    [StringLength(128, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must include uppercase, lowercase, and a number.")]
    string Password);
