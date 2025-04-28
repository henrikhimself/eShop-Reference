namespace Hj.Basket.Entity;

internal sealed class BasketEntity
{
  public required ICollection<BasketItemEntity> Items { get; set; }
}
