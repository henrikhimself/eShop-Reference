using System.ComponentModel.DataAnnotations;

namespace Hj.Commerce.Features.CommerceContentModel.BaseContent;

internal abstract class MovieVariationBase : SiteVariationBase
{
  [Display(
    Name = "Release Year",
    Order = 100)]
  public virtual string? ReleaseYear { get; set; }

  [Display(
    Name = "Duration (minutes)",
    Order = 110)]
  public virtual int? DurationMinutes { get; set; }
}
