using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives
{
    public interface ICodeRenderer
    {
        void Render(ICompiler compiler);
    }
}
