using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives
{
    public interface IInvocationBuilder 
    {
        IInvocationBuilder WithParam(string value);

        IInvocationBuilder WithParam(INamedReference valueReference);

        IInvocationBuilder WithParam(Action<ICodeBlockBuilder> paramCode);
    }
}
