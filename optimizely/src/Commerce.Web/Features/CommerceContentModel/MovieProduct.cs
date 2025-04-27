using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Web;
using Hj.Commerce.Features.CmsContentModel;
using Hj.Commerce.Features.CommerceContentModel.BaseContent;

namespace Hj.Commerce.Features.CommerceContentModel;

[CatalogContentType(
  DisplayName = "Movie",
  GUID = "e9e0b060-1bc8-46a4-86ea-7ad68de19c4c",
  MetaClassName = nameof(MovieProduct))]
public class MovieProduct : SiteProductBase
{
  [CultureSpecific]
  [Display(
    Name = "Poster Image",
    Order = 100)]
  [AllowedTypes(typeof(ImageMedia))]
  [UIHint(UIHint.Image)]
  public virtual ContentReference? PosterImage { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Trailer Video",
    Order = 110)]
  [AllowedTypes(typeof(VideoMedia))]
  [UIHint(UIHint.Video)]
  public virtual ContentReference? TrailerVideo { get; set; }

  [CultureSpecific]
  [Display(
    Name = "Description",
    Order = 120)]
  [UIHint(UIHint.Textarea)]
  public virtual string? Description { get; set; }
}
