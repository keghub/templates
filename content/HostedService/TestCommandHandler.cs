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
            return Task.CompletedTask;
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
            return Task.CompletedTask;
        }
    }
#endif
}