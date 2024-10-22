using EventBus.Message.Events;
using MassTransit;

namespace Basket.Api.EventBusConsumer
{
    public class HadiTestConsumer : IConsumer<HadiTestEvent>
    {
        private readonly ILogger<HadiTestConsumer> _logger;

        public HadiTestConsumer(ILogger<HadiTestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<HadiTestEvent> context)
        {
            var data = context.Message.Name;
            
            _logger.LogInformation($"-------------------{context.Message.Name}---------------");
        }
    }
}
