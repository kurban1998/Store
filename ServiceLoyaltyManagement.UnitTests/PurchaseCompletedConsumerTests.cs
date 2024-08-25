using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.Basket;

namespace ServiceLoyaltyManagement.UnitTests
{
    [TestClass]
    public class PurchaseCompletedConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<ConsumeContext<BasketUserMessage>>();
            _repository = new Mock<ICustomerRepository>(MockBehavior.Strict);
            _target = new PurchaseCompletedConsumer(_repository.Object);
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var userName = _fixture.Create<string>();
            var basket = _fixture.Build<BasketUser>()
                .With(x => x.UserName, userName)
                .Create();

            var request = _fixture.Build<BasketUserMessage>()
                .With(x => x.Basket, basket)
                .Create();

            var customer = _fixture.Build<Customer>()
                .With(x => x.Name, basket.UserName)
                .Create();
            
            _context.Setup(x => x.Message).Returns(request);

            _repository.Setup(x => x.GetByUserName(userName)).Returns(customer);
            _repository.Setup(x => x.Update(customer));
            _repository.Setup(x => x.GetByUserName(userName)).Returns(customer);
            _repository.Setup(x => x.Update(customer));

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.GetByUserName(userName), Times.Exactly(2));
            _repository.Verify(x => x.Update(customer), Times.Exactly(2));
        }

        [TestMethod]
        public void ConsumeIsNull()
        {
            // arrange
            var userName = _fixture.Create<string>();
            var basket = _fixture.Build<BasketUser>()
                .With(x => x.UserName, userName)
                .Create();

            var request = _fixture.Build<BasketUserMessage>()
                .With(x => x.Basket, basket)
                .Create();

            var newCustomer = _fixture.Build<Customer>()
                .With(x => x.Name, basket.UserName)
                .With(x => x.Address, basket.Address)
                .With(x => x.Bonuses, 0)
                .With(x => x.PurchaseNumber, basket.Products.Count())
                .Without(x => x.Id)
                .Create();

            _context.Setup(x => x.Message).Returns(request);
            Customer customer = null;
            
            _repository.Setup(x => x.GetByUserName(userName)).Returns(customer!);
            _repository.Setup(x => x.Add(It.IsAny<Customer>()));

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.GetByUserName(userName), Times.Exactly(2));
            _repository.Verify(x => x.Add(It.IsAny<Customer>()), Times.Once);
        }

        private Mock<ConsumeContext<BasketUserMessage>> _context;
        private Mock<ICustomerRepository> _repository;
        private PurchaseCompletedConsumer _target;
        
        private readonly Fixture _fixture = new Fixture();
    }
}