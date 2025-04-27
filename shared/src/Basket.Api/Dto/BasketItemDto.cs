namespace Hj.Basket.Dto;

public sealed class BasketItemDto
{
  public required string Sku { get; set; }
  public required string Name { get; set; }
  public decimal Price { get; set; }
  public required string Currency { get; set; }
  public int Quantity { get; set; }
}
