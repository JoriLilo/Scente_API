namespace Scente.API.DTOs;

public class ApplyPromoDto {
    public string Code { get; set; } = string.Empty;
}

public class CartMergeDto {
    public int UserId { get; set; }
    public string GuestCartId { get; set; } = string.Empty;
}

public class PromoDto
{
    public string Code { get; set; } = string.Empty;
}
