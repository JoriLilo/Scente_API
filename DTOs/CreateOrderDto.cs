using System.ComponentModel.DataAnnotations;

namespace Scente.API.DTOs;

// ============================================================
// CreateOrderDto
// What the frontend (checkout.js) is allowed to send.
// We deliberately do NOT accept cart items or any total — items
// come from the user's DB cart and the total is calculated
// server-side, so nobody can fake a cheap order.
//
// Week 3: server-side address validation. [Required] means the
// API rejects the order with a 400 if these are blank, even if a
// user bypasses the frontend's "required" attributes.
// ============================================================
public class CreateOrderDto
{
    // "card" or "cod" (cash on delivery)
    public string PaymentMethod { get; set; } = "cod";

    [Required(ErrorMessage = "Shipping address is required.")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    public string Phone { get; set; } = string.Empty;
}