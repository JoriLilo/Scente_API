namespace Scente.API.Services;

// ============================================================
// IEmailService
// Abstraction so the controller depends on an interface, not on
// MailKit directly. Makes the controller testable (you can swap
// in a fake email service in Week 4 unit tests).
// ============================================================
public interface IEmailService
{
    // Sends the order confirmation. Returns true on success, false
    // if sending failed — the caller decides what to do, but a
    // failed email must never roll back a saved order.
    Task<bool> SendOrderConfirmationAsync(OrderEmailData data);
}

// Plain data carrier with everything the email template needs.
public class OrderEmailData
{
    public string ToEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalPaid { get; set; }
    public string EstimatedDelivery { get; set; } = string.Empty;
    public List<OrderEmailLine> Items { get; set; } = new();
}

public class OrderEmailLine
{
    public string ProductName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
