using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives
{
    public interface IPropertyBuilder : ICodeRenderer, INamedReference, IWithModifiers<IPropertyBuilder>
    {
        ISetterBuilder HasSetter();

        ICodeBlockBuilder HasGetter();

        void Returns(Action<ICodeBlockBuilder> value);
    }
}
