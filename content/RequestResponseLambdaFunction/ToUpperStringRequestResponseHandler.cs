using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using EMG.Extensions.Logging.Loggly;
using Kralizek.Lambda;
using Microsoft.Extensions.Logging;

namespace EMG
{
    public class ToUpperStringRequestResponseHandler : IRequestResponseHandler<string, string>
    {
        private readonly ILogger<ToUpperStringRequestResponseHandler> _logger;
        private readonly ILogglyProcessor _logglyProcessor;

        public ToUpperStringRequestResponseHandler(ILogger<ToUpperStringRequestResponseHandler> logger, ILogglyProcessor logglyProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logglyProcessor = logglyProcessor ?? throw new ArgumentNullException(nameof(logglyProcessor));
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
                throw;
            }
            finally
            {
                _logglyProcessor.FlushMessages(); // ensure all logs are sent to Loggly before the application terminates.
            }
        }
    }
}