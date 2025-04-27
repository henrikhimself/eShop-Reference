using EPiServer.Commerce.Catalog.DataAnnotations;
using Hj.Commerce.Features.CommerceContentModel.BaseContent;

namespace Hj.Commerce.Features.CommerceContentModel;

[CatalogContentType(
  DisplayName = "Bundle",
  GUID = "6225341c-ed52-41e7-acc3-436dc0661a1f",
  MetaClassName = nameof(MovieBundle))]
public class MovieBundle : SiteBundleBase
{
}
