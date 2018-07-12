using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Robowire.RoboApi.Convention
{
    public interface IParameterReader
    {
        T Read<T>(ParameterInfo parameter, RequestContext context);
    }
    
}
