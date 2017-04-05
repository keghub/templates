using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Common;
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

            container.Register(Component.For<WcfHostingOptions>().FromConfiguration(c => c.GetSection("WCF")));
        }
    }
    
}