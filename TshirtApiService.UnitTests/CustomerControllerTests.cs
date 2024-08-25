using AutoFixture;
using Database.Models;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharedModels.CustomerService;
using ThisrtApiService.Controllers;

namespace TshirtApiService.UnitTests
{
    [TestClass]
    public class CustomerControllerTests
    {

        [TestMethod]
        public async Task GetUserByName()
        {
            // arrange
            var userName = _fixture.Create<string>();
            var customer = _fixture.Build<Customer>()
                .With(x => x.Name, userName)
                .Create();

            await using var provider = new ServiceCollection()
             .AddMassTransitTestHarness(cfg =>
             {
                  cfg.AddHandler<CustomerRequest>(async cxt =>
                  {
                     await cxt.RespondAsync(new CustomerResponce { Customer = customer});
                  });
             })
             .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            // act
            var client = harness.GetRequestClient<CustomerRequest>();

            var response = await client.GetResponse<CustomerResponce>(new CustomerRequest
            {
                UserName = userName
            });

            var messageSent = harness.Sent.Select<CustomerResponce>().FirstOrDefault();
            var message = messageSent.Context.Message.Customer;

            // assert
            response.Message.Customer.Should().BeEquivalentTo(customer);
            message.Should().BeEquivalentTo(customer);
        }

        private readonly Fixture _fixture = new Fixture();
    }
}
