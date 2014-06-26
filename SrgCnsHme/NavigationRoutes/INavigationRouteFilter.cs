using System.Web.Routing;

namespace SrgCnsHme.NavigationRoutes
{
  public interface INavigationRouteFilter
  {
    bool ShouldRemove(Route navigationRoutes);
  }
}
