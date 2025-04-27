using EPiServer.Commerce.Catalog.DataAnnotations;
using Hj.Commerce.Features.CommerceContentModel.BaseContent;

namespace Hj.Commerce.Features.CommerceContentModel;

[CatalogContentType(
  DisplayName = "Package",
  GUID = "16f37971-ea5f-4ca7-b801-a6be58097a92",
  MetaClassName = nameof(MoviePackage))]
public class MoviePackage : SitePackageBase
{
}
