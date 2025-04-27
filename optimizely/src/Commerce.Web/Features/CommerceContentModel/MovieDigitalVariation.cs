using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.DataAnnotations;
using Hj.Commerce.Features.CommerceContentModel.BaseContent;

namespace Hj.Commerce.Features.CommerceContentModel;

[CatalogContentType(
  DisplayName = "Digital",
  GUID = "58cd9ed8-2420-4177-8494-763cc3b667ae",
  MetaClassName = nameof(MovieDigitalVariation))]
public class MovieDigitalVariation : MovieVariationBase
{
  [Display(
    Name = "EntitlementKey",
    Order = 100)]
  public virtual string? EntitlementKey { get; set; }
}
