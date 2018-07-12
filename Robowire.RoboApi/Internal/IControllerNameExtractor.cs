using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Robowire.RoboApi.Internal
{
    public interface IControllerNameExtractor
    {
        string GetControllerName(RequestContext requestContext, string defaultControllerName);
    }
}
