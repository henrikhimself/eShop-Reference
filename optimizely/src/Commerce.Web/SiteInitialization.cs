using EPiServer.Commerce.Initialization;
using EPiServer.Commerce.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Hj.Commerce;

[InitializableModule]
[ModuleDependency(typeof(InitializationModule))]
public class SiteInitialization : IInitializableModule
{
  public void Initialize(InitializationEngine context)
  {
    CatalogRouteHelper.MapDefaultHierarchialRouter(false);
  }

  public void Uninitialize(InitializationEngine context)
  {
  }
}
