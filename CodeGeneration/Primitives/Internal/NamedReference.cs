using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.Primitives.Internal
{
    internal class NamedReference : INamedReference
    {
        public NamedReference(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
    }
}
