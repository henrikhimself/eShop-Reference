using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace Hj.Commerce.Features.CommerceContentModel.BaseContent;

internal abstract class SiteBundleBase : BundleContent
{
  [CultureSpecific]
  [Display(
    Name = "Disable indexing",
    Description = "Exclude content from being included when searching.",
    GroupName = SystemTabNames.PageHeader,
    Order = 100)]
  public virtual bool DisableIndexing { get; set; }
}
