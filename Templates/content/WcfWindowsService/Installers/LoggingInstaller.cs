using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EMG.Common;
using EMG.Wcf.Installers;
using Loggly.Config;
using NLog;
using Nybus.Logging;
using ILogger = Nybus.Logging.ILogger;

namespace EMG.WcfWindowsService.Installers
{
    public class LoggingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILoggerFactory>().ImplementedBy<LoggerFactory>().OnCreate(ConfigureLoggerFactory).LifeStyle.Singleton);

            container.Register(Component.For<ILogger>().CreateLoggerForTargetClass());

            container.Register(Component.For<LogglyOptions>().FromConfiguration(r => r.GetSection("Loggly")));
        }

        private void ConfigureLoggerFactory(ILoggerFactory loggerFactory)
        {
            var logFactory = new LogFactory();
            loggerFactory.AddProvider(new NLogLoggerProvider(logFactory));
        }

    }

    public class LogglyOptions
    {
        public string EndpointHostname { get; set; } = "logs-01.loggly.com";

        public int EndpointPort { get; set; } = 443;

        public LogTransport LogTransport { get; set; } = LogTransport.Https;

        public string CustomerToken { get; set; }
    }
}