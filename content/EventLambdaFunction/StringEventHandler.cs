using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Kralizek.Lambda;
using Microsoft.Extensions.Logging;

namespace EMG
{
    public class StringEventHandler : IEventHandler<string>
    {
        private readonly ILogger<StringEventHandler> _logger;

        public StringEventHandler(ILogger<StringEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                Thread.Sleep(1000); // Thread.Sleep to ensure all logs are sent to Loggly before the application terminates.
            }
        }
    }
}