using System;
using System.Threading.Tasks;
using EMG.WcfWindowsServiceWithNybus.Messages;
using Nybus;
using Nybus.Logging;

namespace EMG.WcfWindowsServiceWithNybus
{
    /*
        The concrete implementation of your service.
        It can implement more than one service contracts.
        It can use IBus to push messages to Nybus.
    */
    public class WcfWindowsServiceWithNybus : IWcfWindowsServiceWithNybus
    {
        private readonly IBus _bus;
        private readonly ILogger _logger;

        public WcfWindowsServiceWithNybus(IBus bus, ILogger logger)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PushMessageToQueue(string message)
        {
            _logger.LogInformation(new { message }, s => $"Received: `{message}`");
            await _bus.RaiseEvent(new MessageReceived {Message = message});
        }
    }
}