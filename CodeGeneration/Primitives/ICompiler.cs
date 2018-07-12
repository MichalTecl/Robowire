using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives
{
    public interface ICompiler
    {
        ICompiler Write(string value);

        ICompiler WriteLine(string value = null);

        void RegisterType(Type type);

        Assembly Compile();
    }
}
