using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Kralizek.Lambda;
using Microsoft.Extensions.Logging;

namespace EMG
{
    public class ToUpperStringRequestResponseHandler : IRequestResponseHandler<string, string>
    {
        private readonly ILogger<ToUpperStringRequestResponseHandler> _logger;

        public ToUpperStringRequestResponseHandler(ILogger<ToUpperStringRequestResponseHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> HandleAsync(string input, ILambdaContext context)
        {
            try
            {
                await Task.Delay(1);

                return input?.ToUpper();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error in HandleAsync for {nameof(ToUpperStringRequestResponseHandler)}");
                
                Thread.Sleep(1000); // Thread.Sleep to ensure all logs are sent to Loggly before the application terminates.
                throw;
            }
        }
    }
}