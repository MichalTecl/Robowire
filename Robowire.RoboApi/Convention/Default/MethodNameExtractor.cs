using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Robowire.RoboApi.Convention.Default
{
    public static class MethodNameExtractor
    {
        public static string ExtractMethodName(RequestContext requestContext)
        {
            var routes = requestContext.RouteData.Values;
            return (routes["action"] as string)?.Trim().ToLowerInvariant() ?? String.Empty;
        }
    }
}
