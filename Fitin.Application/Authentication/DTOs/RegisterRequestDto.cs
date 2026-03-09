namespace Fitin.Application.DTOs;

public record RegisterRequestDto(
    string Name,
    string Email,
    string Password);