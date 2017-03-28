using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Wcf.Installers;
using NLog;
using Nybus.Logging;
using ILogger = Nybus.Logging.ILogger;

namespace EMG.WcfWindowsServiceWithDiscovery.Installers
{
    public class LoggingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILoggerFactory>().ImplementedBy<LoggerFactory>().OnCreate(ConfigureLoggerFactory).LifeStyle.Singleton);

            container.Register(Component.For<ILogger>().CreateLoggerForTargetClass());
        }

        private void ConfigureLoggerFactory(ILoggerFactory loggerFactory)
        {
            var logFactory = new LogFactory();
            loggerFactory.AddProvider(new NLogLoggerProvider(logFactory));
        }

    }
}