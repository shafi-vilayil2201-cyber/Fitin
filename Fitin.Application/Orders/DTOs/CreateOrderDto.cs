using System;
using System.Collections.Generic;

namespace Fitin.Application.Orders.DTOs;

public class CreateOrderDto
{
    public string ShippingName { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    public string ShippingPhone { get; set; } = string.Empty;
    
    // Updated to support multiple items
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}

