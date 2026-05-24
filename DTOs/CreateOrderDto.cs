namespace Scente.API.DTOs;

// ============================================================
// CreateOrderDto
// What the frontend (checkout.js) is allowed to send us.
// NOTE: We deliberately do NOT accept the cart items or the
// total from the client. The items come from the user's cart
// in the DB, and the total is calculated server-side. This
// stops anyone from faking a $1 order in the browser.
// ============================================================
public class CreateOrderDto
{
    // "card" or "cod" (cash on delivery)
    public string PaymentMethod { get; set; } = "cod";

    // Shipping address details (snapshot saved onto the order)
    public string ShippingAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
