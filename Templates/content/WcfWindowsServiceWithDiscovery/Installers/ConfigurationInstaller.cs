using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Common;
using Microsoft.Extensions.Configuration;

namespace EMG.WcfWindowsServiceWithDiscovery.Installers
{
    public class ConfigurationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddConfiguration(
                b => b.AddJsonFile("appsettings.json", true),
                b => b.AddEnvironmentVariables()
            );

            // Check https://app.assembla.com/spaces/studentum/git-8/source/master/Configuration/CastleWindsor/README.md for usages
        }
    }

}
