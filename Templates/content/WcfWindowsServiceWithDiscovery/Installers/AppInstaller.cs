using System;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Wcf.Installers;

namespace EMG.Installers
{
    public class AppInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.RegisterWcfService<EchoService, EchoServiceHostConfigurator>();

            /* EQUIVALENT TO
            container.Register(Component.For<EchoService>().LifestyleTransient());

            container.Register(Component.For<IServiceHostConfigurator<EchoService>, EchoServiceHostConfigurator>());
            */
        }
    }
    
}