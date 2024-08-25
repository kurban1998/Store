using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.CustomerService;

namespace CustomerManagementService.UnitTests
{
    [TestClass]
    public class GetCustomerConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<ConsumeContext<CustomerRequest>>();
            _repository = new Mock<ICustomerRepository>(MockBehavior.Strict);
            _target = new GetCustomerConsumer(_repository.Object);
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var userName = _fixture.Create<string>();
            var customer = _fixture.Build<Customer>()
                .With(x => x.Name, userName)
                .Create();

            var request = _fixture.Build<CustomerRequest>()
                .With(x => x.UserName, userName)
                .Create();

            var responce = _fixture.Build<CustomerResponce>()
                .With(x => x.Customer, customer)
                .Create();

            _context.Setup(x => x.Message).Returns(request);
            _context.Setup(x => x.RespondAsync<CustomerResponce>(responce)).Returns(Task.CompletedTask);
            _repository.Setup(x => x.GetByUserName(userName)).Returns(customer);

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.GetByUserName(userName), Times.Once);
            _context.Verify(x => x.Message, Times.Once);
        }

        private GetCustomerConsumer _target;
        private Mock<ICustomerRepository> _repository;
        private Mock<ConsumeContext<CustomerRequest>> _context;

        private readonly Fixture _fixture = new Fixture();
    }
}