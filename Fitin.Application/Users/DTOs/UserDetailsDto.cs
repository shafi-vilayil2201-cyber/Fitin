

using Fitin.Application.Orders.DTOs;
using Fitin.Domain.Enums;

namespace Fitin.Application.Users.DTOs;

public class UserDetailsDto
{
    public Guid Id {get;set;}
    public string Name{get; set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public UserRole Role {get;set;}
    public bool IsActive {get;set;}
    public DateTime CreatedAt{get;set;}
    public List<OrderDto> Orders {get;set;} = new();
}

    
