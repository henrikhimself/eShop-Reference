using EPiServer.Shell;

namespace Hj.Commerce.Features.CmsContentModel.Descriptors;

[UIDescriptorRegistration]
public class FolderPageUIDescriptor : UIDescriptor<FolderPage>
{
  public FolderPageUIDescriptor()
       : base(ContentTypeCssClassNames.Container)
  {
    DefaultView = CmsViewNames.ContentListingView;
    EnableStickyView = true;
    DisabledViews = [
      CmsViewNames.AllPropertiesView,
      CmsViewNames.OnPageEditView,
      CmsViewNames.AllPropertiesCompareView,
      CmsViewNames.SideBySideCompareView,
      CmsViewNames.PreviewView,
    ];
  }
}
