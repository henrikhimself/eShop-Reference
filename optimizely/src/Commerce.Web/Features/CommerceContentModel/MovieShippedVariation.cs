using EPiServer.Commerce.Catalog.DataAnnotations;
using Hj.Commerce.Features.CommerceContentModel.BaseContent;

namespace Hj.Commerce.Features.CommerceContentModel;

[CatalogContentType(
  DisplayName = "Variation",
  GUID = "a41c5610-f520-4700-be07-af87c99288a9",
  MetaClassName = nameof(MovieShippedVariation))]
public class MovieShippedVariation : MovieVariationBase
{
}
