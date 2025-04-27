using Hj.Basket.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hj.Basket;

public static class BasketApi
{
  public static IEndpointRouteBuilder MapBasketApi(this IEndpointRouteBuilder app)
  {
    var api = app.NewVersionedApi()
      .WithTags("Basket")
      .RequireAuthorization("ApiScope");
    var v1 = api.MapGroup("api").HasApiVersion(1, 0);

    v1.MapGet("/basket", GetBasket)
      .WithName("Basket")
      .WithSummary("Get basket data");

    return app;
  }

  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
  public static Ok<BasketDto> GetBasket()
  {
    var basket = new BasketDto()
    {
      Basket = new BasketOutputDto()
      {
        Items = [
          new()
          {
            Sku = "1234",
            Name = "Floppy 5 1/4 360kb",
            Price = 2000,
            Currency = "DKK",
            Quantity = 15,
          }
        ]
      }
    };

    return TypedResults.Ok(basket);
  }
}
