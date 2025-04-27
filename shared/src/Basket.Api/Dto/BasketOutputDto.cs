namespace Hj.Basket.Dto;

public sealed class BasketOutputDto
{
  public required ICollection<BasketItemDto> Items { get; set; }
}
