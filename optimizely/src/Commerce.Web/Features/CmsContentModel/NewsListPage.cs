using Hj.Commerce.Features.CmsContentModel.BaseContent;

namespace Hj.Commerce.Features.CmsContentModel;

[ContentType(
  DisplayName = "News List",
  GUID = "d28e12d0-32ee-4a1a-a5fe-98b271bf4069")]
public class NewsListPage : SitePageBase
{
  public virtual IList<NewsItemBlock>? NewsItems { get; set; }
}
