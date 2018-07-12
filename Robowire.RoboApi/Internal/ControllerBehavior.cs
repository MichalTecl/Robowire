using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.Core;

namespace Robowire.RoboApi.Internal
{
    public class ControllerBehavior : IBehavior
    {
        public string ControllerName { get; set; }

        public Type ProxyBuilderType { get; set; }

        public Type CallBuilderType { get; set; }

        public void BindTo(IServiceSetupRecord setup)
        {
        }

        public void InheritPreviousBehavior(IBehavior behavior)
        {
        }
    }
}
