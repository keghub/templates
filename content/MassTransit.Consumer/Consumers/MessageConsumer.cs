using MassTransit.Publisher.Handlers;

namespace MassTransit.Publisher.Consumers;

public class MessageConsumer<TMessage> : IConsumer<TMessage>
    where TMessage : class
{
    private readonly IMessageHandler<TMessage> _messageHandler;

    public MessageConsumer(IMessageHandler<TMessage> messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await _messageHandler.HandleAsync(context.Message);
    }
}