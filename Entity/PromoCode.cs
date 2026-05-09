namespace Scente.API.Entity;

public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;       // e.g. "SCENTE10"
    public decimal DiscountRate { get; set; }               // e.g. 0.10 = 10%
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }                // null = never expires
}