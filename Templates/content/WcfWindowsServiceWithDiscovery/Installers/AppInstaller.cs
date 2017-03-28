using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Wcf.Installers;

namespace EMG.WcfWindowsServiceWithDiscovery.Installers
{
    public class AppInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.RegisterWcfService<WcfWindowsServiceWithDiscovery, WcfWindowsServiceWithDiscoveryServiceHostConfigurator>();

            /* EQUIVALENT TO
            container.Register(Component.For<WcfWindowsServiceWithDiscovery>().LifestyleTransient());

            container.Register(Component.For<IServiceHostConfigurator<WcfWindowsServiceWithDiscovery>, WcfWindowsServiceWithDiscoveryServiceHostConfigurator>());
            */
        }
    }
    
}