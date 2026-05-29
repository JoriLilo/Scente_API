namespace Scente.API.DTOs;

// One row in the orders list
public class OrderSummaryDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalPaid { get; set; }
    public int ItemCount { get; set; }
}

// One line on an order
public class OrderItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;
}

// Full order detail (when you open a single order)
public class OrderDetailDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal TotalPaid { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}

// Tab badge counts
public class OrderCountsDto
{
    public int All { get; set; }
    public int Pending { get; set; }
    public int Shipped { get; set; }
    public int Delivered { get; set; }
}
