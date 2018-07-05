using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives
{
    public interface IConstructorBuilder : IMethodBuilder, IWithModifiers<IConstructorBuilder>
    {
        IInvocationBuilder CallsBase();

        new IConstructorBuilder WithModifier(string modifier);
    }
}
