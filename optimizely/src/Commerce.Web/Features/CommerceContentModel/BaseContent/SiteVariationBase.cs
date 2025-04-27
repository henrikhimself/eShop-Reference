using System.ComponentModel.DataAnnotations;

namespace Hj.Commerce.Features.CommerceContentModel.BaseContent;

public abstract class SiteVariationBase
{
  [CultureSpecific]
  [Display(
    Name = "Disable indexing",
    Description = "Exclude content from being included when searching.",
    GroupName = SystemTabNames.PageHeader,
    Order = 100)]
  public virtual bool DisableIndexing { get; set; }
}
