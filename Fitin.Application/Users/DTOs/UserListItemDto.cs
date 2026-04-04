using System.Text.Json.Serialization;
using Fitin.Domain.Enums;

namespace Fitin.Application.Users.DTOs;

public class UserListItemDto
{
    public Guid Id{get; set;}
    public string Name{get; set;} = string.Empty;
    public string Email{get;set;} = string.Empty;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role{ get;set;}
    public bool IsActive{get;set;}
    public DateTime CreatedAt{get;set;}
}
