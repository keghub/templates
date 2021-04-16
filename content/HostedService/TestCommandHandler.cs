using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
#if (NybusLegacy || NybusCurrent)
using Nybus;
#endif

namespace EMG.Hosted_Service
{
#if (NybusLegacy || NybusCurrent)
    public class TestCommand : ICommand { }
#endif

#if (NybusCurrent)
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        private readonly ILogger<TestCommandHandler> _logger;

        public TestCommandHandler(ILogger<TestCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task HandleAsync(IDispatcher dispatcher, ICommandContext<TestCommand> context)
        {
            try 
            {	        
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error in HandleAsync for {nameof(TestCommandHandler)}");
                throw;
            }
        }
    }
#endif
#if (NybusLegacy)
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        private readonly ILogger<TestCommandHandler> _logger;

        public TestCommandHandler(ILogger<TestCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(CommandContext<TestCommand> commandMessage)
        {
            try 
            {	        
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error in HandleAsync for {nameof(TestCommandHandler)}");
                throw;
            }
        }
    }
#endif
}