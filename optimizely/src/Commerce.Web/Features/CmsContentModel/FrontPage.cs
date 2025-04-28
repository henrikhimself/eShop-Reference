using System.ComponentModel.DataAnnotations;
using EPiServer.Web;
using Hj.Commerce.Features.CmsContentModel.BaseContent;

namespace Hj.Commerce.Features.CmsContentModel;

[ContentType(
  DisplayName = "Frontpage",
  GUID = "3c952f63-1e40-4288-8545-548ce6d103d4")]
internal class FrontPage : SitePageBase
{
  [CultureSpecific]
  [Display(
    Name = "Hero Header",
    Order = 100)]
  public virtual string? HeroHeader { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Hero Byline",
    Order = 110)]
  public virtual string? HeroByline { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Hero Image",
    Order = 120)]
  [AllowedTypes(typeof(ImageMedia))]
  [UIHint(UIHint.Image)]
  public virtual ContentReference? HeroImage { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Hero Content",
    Order = 130)]
  public virtual ContentArea? HeroContent { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Watch On TV Content",
    Order = 200)]
  public virtual XhtmlString? WatchOnTvContent { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Watch On TV Image",
    Order = 210)]
  [AllowedTypes(typeof(ImageMedia))]
  [UIHint(UIHint.Image)]
  public virtual ContentReference? WatchOnTvImage { get; set; }
}
