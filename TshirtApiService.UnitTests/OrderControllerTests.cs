using AutoFixture;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.Basket;
using SharedModels.CustomerService;

namespace TshirtApiService.UnitTests
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public async Task CreateOrder()
        {
            // arrange
            var basket = _fixture.Create<BasketUser>();

            await using var provider = new ServiceCollection()
             .AddMassTransitTestHarness(cfg =>
             {
                 cfg.AddHandler<BasketUserMessage>(async cxt =>
                 {
                     await cxt.RespondAsync(new BasketUserMessage
                     {
                         Basket = basket
                     });
                 });
             })
             .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            // act
            var client = harness.GetRequestClient<BasketUserMessage>();

            var response = await client.GetResponse<BasketUserMessage>(new BasketUserMessage
            {
                Basket = basket
            });

            var messageSent = harness.Sent.Select<BasketUserMessage>().FirstOrDefault();
            var message = messageSent.Context.Message.Basket;

            // assert
            var a = response.Message;
            response.Message.Basket.Should().BeEquivalentTo(message);
        }

        private readonly Fixture _fixture = new Fixture();
    }
}
