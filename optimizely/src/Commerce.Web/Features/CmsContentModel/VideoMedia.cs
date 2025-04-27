using EPiServer.Framework.DataAnnotations;

namespace Hj.Commerce.Features.CmsContentModel;

[ContentType(
  DisplayName = "Video",
  GUID = "0f3d70f0-f21e-4bbb-ab36-1de15927b2b9")]
[MediaDescriptor(ExtensionString = "mp4,webm")]
public class VideoMedia : VideoData
{
}
