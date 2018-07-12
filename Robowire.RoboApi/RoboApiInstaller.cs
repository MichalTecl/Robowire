using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Robowire.RoboApi.Internal;

namespace Robowire.RoboApi
{
    public class RoboApiInstaller
    {
        public void Install(
            ControllerBuilder controllerBuilder,
            IContainer container,
            params Assembly[] controllerAssemblies)
        {
            container.Setup(
                setup =>
                    {
                        setup.RegisterPlugin(
                            ps => ps.CustomInstanceCreators.Add(new ControllerIndex.ControllerIndexPlugin()));

                        RegisterControllerNameExtractor(setup);

                        foreach (var asm in controllerAssemblies)
                        {
                            setup.ScanAssembly(asm);
                        }

                        setup.For<ControllerIndex>().Use<ControllerIndex>();
                    });

            var factory = new RaControllerFactory(container);

            controllerBuilder?.SetControllerFactory(factory);
        }

        protected virtual void RegisterControllerNameExtractor(IContainerSetup setup)
        {
            setup.For<IControllerNameExtractor>().Use<DefaultControllerNameExtractor>();
        }
    }
}
