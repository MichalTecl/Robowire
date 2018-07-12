using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeGeneration;
using CodeGeneration.Primitives;

namespace Robowire.RoboApi.Convention
{
    public interface IControllerMethodCallBuilder
    {
        void BuildCall(
            IClassBuilder proxy,
            IMethodBuilder executeMethod,
            INamedReference contextParameter,
            Type controllerType,
            Func<Type, INamedReference> privateObjectsFactory,
            INamedReference interceptorField);
    }
}
