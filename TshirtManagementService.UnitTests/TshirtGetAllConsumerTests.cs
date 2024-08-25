using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService.UnitTests
{
    [TestClass]
    public class TshirtGetAllConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _context = new Mock<ConsumeContext<TshirtMessageGetAllRequest>>();
            _repository = new Mock<ITshirtRepository>();
            _target = new TshirtGetAllConsumer(_repository.Object);
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var tshirts = _fixture.CreateMany<Tshirt>().AsQueryable();
            _repository.Setup(x => x.GetAll()).Returns(tshirts);

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.GetAll(), Times.Once);
        }

        private TshirtGetAllConsumer _target;
        private Mock<ITshirtRepository> _repository;
        private Mock<ConsumeContext<TshirtMessageGetAllRequest>> _context;
        private readonly Fixture _fixture = new Fixture();
    }
}
