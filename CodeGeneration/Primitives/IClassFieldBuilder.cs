using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives
{
    public interface IClassFieldBuilder : INamedReference, ICodeRenderer, IWithModifiers<IClassFieldBuilder>
    {
        IClassFieldBuilder WithAssignment(Action<ICodeBlockBuilder> assignment);
    }
}
