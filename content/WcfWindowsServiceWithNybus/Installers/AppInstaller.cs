using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Common;
using EMG.Wcf.Installers;

namespace EMG.WcfWindowsServiceWithNybus.Installers
{
    public class AppInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ServiceHostWrapper<WcfWindowsServiceWithNybus>>());

            container.RegisterWcfService<WcfWindowsServiceWithNybus, WcfWindowsServiceWithNybusServiceHostConfigurator>();

            /* EQUIVALENT TO
            container.Register(Component.For<WcfWindowsServiceWithNybus>().LifestyleTransient());

            container.Register(Component.For<IServiceHostConfigurator<WcfWindowsServiceWithNybus>, WcfWindowsServiceWithNybusServiceHostConfigurator>());
            */

            container.Register(Component.For<WcfHostingOptions>().FromConfiguration(c => c.GetSection("WCF")));
        }
    }
}