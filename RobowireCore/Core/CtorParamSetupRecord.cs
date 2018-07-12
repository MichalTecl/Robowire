using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.Core
{
    public class CtorParamSetupRecord
    {
        public Type ParameterType { get; set; }

        public string ParameterName { get; set; }

        public NamedFactory ValueProvider { get; set; }
     }
}
