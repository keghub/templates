using System;
using Nybus.Logging;

namespace EMG.WcfWindowsServiceWithDiscovery
{

    /*
        The concrete implementation of your service.
        It can implement more than one service contract.
    */
    public class WcfWindowsServiceWithDiscovery : IWcfWindowsServiceWithDiscovery
    {
        private readonly ILogger _logger;

        public WcfWindowsServiceWithDiscovery(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    }
}