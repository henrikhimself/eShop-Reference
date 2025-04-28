namespace Hj.Basket.Dto;

internal sealed class BasketOutputDto
{
  public required ICollection<BasketItemDto> Items { get; set; }
}
