using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using EMG.Extensions.Logging.Loggly;
using Kralizek.Lambda;
using Microsoft.Extensions.Logging;

namespace EMG
{
    public class StringEventHandler : IEventHandler<string>
    {
        private readonly ILogger<StringEventHandler> _logger;
        private readonly ILogglyProcessor _logglyProcessor;

        public StringEventHandler(ILogger<StringEventHandler> logger, ILogglyProcessor logglyProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logglyProcessor = logglyProcessor ?? throw new ArgumentNullException(nameof(logglyProcessor));
        }

        public Task HandleAsync(string input, ILambdaContext context)
        {
            try
            {
                _logger.LogInformation($"Received: {input}");

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error in HandleAsync for {nameof(StringEventHandler)}");
                throw;
            }
            finally
            {
                _logglyProcessor.FlushMessages(); // ensure all logs are sent to Loggly before the application terminates.
            }
        }
    }
}