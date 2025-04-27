using System.ComponentModel.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Hj.Commerce.Features.CmsContentModel.BaseContent;

namespace Hj.Commerce.Features.CmsContentModel;

[ContentType(
  DisplayName = "Landing Page",
  GUID = "c996e027-1ac1-4221-9f54-c7b8e1236f16")]
public class LandingPage : SitePageBase
{
  [CultureSpecific]
  [Display(
    Name = "Image",
    Order = 100)]
  [AllowedTypes(typeof(ImageMedia))]
  [UIHint(UIHint.Image)]
  public virtual ContentReference? Image { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Heading",
    Order = 110)]
  public virtual string? Heading { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Main Body",
    Order = 120)]
  public virtual XhtmlString? MainBody { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Call To Action Link",
    Order = 130)]
  public virtual LinkItem? CtaLink { get; set; }
}
