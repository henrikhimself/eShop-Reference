using EPiServer.Commerce.Catalog.DataAnnotations;
using Hj.Commerce.Features.CommerceContentModel.BaseContent;

namespace Hj.Commerce.Features.CommerceContentModel;

[CatalogContentType(
  DisplayName = "Director",
  GUID = "e81b514e-7ba5-430f-9075-94aeedd1c3f5",
  MetaClassName = nameof(MovieDirectorCategory))]
internal class MovieDirectorCategory : SiteNodeBase
{
}
