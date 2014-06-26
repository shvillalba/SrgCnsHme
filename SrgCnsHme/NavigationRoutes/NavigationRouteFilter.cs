using System.Web.Routing;

namespace SrgCnsHme.NavigationRoutes
{
  public class NavigationRouteFilter : INavigationRouteFilter
  {
    public bool ShouldRemove(Route route)
    {
      return true;
    }
  }
}
