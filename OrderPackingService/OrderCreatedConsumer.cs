using MassTransit;
using SharedModels.Basket;

namespace OrderPackingService
{
    internal class OrderCreatedConsumer : IConsumer<BasketUserMessage>
    {
        public Task Consume(ConsumeContext<BasketUserMessage> context)
        {
            // интеграция с доставкой

            return Task.CompletedTask;
        }
    }
}
