namespace Hj.Basket.Entity;

public class BasketEntity
{
  public required ICollection<BasketItemEntity> Items { get; set; }
}
