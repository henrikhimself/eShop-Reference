using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace Hj.Commerce.Features.CommerceContentModel.BaseContent;

public abstract class SiteNodeBase : NodeContent
{
  [CultureSpecific]
  [Display(
    Name = "Disable indexing",
    Description = "Exclude content from being included when searching.",
    GroupName = SystemTabNames.PageHeader,
    Order = 100)]
  public virtual bool DisableIndexing { get; set; }
}
