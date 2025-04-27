using EPiServer.Framework.DataAnnotations;

namespace Hj.Commerce.Features.CmsContentModel;

[ContentType(
  DisplayName = "Image",
  GUID = "c9da2b70-6050-4f97-807e-b0094c871824")]
[MediaDescriptor(ExtensionString = "gif,jpg,jpeg,jfif,pjpeg,pjp,png,svg,webp")]
public class ImageMedia : ImageData
{
}
