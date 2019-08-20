using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Common;
using EMG.Wcf.Installers;

namespace EMG.WcfWindowsService.Installers
{
    public class AppInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.RegisterWcfService<WcfWindowsService, WcfWindowsServiceServiceHostConfigurator>();

            /* EQUIVALENT TO
            container.Register(Component.For<WcfWindowsService>().LifestyleTransient());

            container.Register(Component.For<IServiceHostConfigurator<WcfWindowsService>, WcfWindowsServiceWithDiscoveryServiceHostConfigurator>());
            */

            container.Register(Component.For<WcfHostingOptions>().FromConfiguration(c => c.GetSection("WCF")));
        }
    }
    
}