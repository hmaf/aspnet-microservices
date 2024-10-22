using EventBus.Message.Events;
using MassTransit;

namespace Basket.Api.EventBusConsumer
{
    public class HadiTestConsumer2 : IConsumer<HadiTestEvent2>
    {
        private readonly ILogger<HadiTestConsumer> _logger;

        public HadiTestConsumer2(ILogger<HadiTestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<HadiTestEvent2> context)
        {
            _logger.LogInformation($"-------------------{context.Message.Name}---------------");
            _logger.LogInformation($"-------------------{context.Message.Family}---------------");
        }
    }
}
