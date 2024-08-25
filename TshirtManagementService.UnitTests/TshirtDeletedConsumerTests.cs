using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService.UnitTests
{
    [TestClass]
    public class TshirtDeletedConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _repository = new Mock<ITshirtRepository>(MockBehavior.Strict);
            _target = new TshirtDeletedConsumer(_repository.Object);
            _context = new Mock<ConsumeContext<TshirtMessageDelete>>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var id = _fixture.Create<int>();
            var tshirt = _fixture.Build<Tshirt>()
                .With(x => x.Id, id)
                .Create();

            var request = _fixture.Build<TshirtMessageDelete>()
                .With(x => x.Id, tshirt.Id)
                .Create();

            _context.Setup(x => x.Message).Returns(request);
            _repository.Setup(x => x.GetById(id)).Returns(tshirt);
            _repository.Setup(x => x.Delete(tshirt));

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.GetById(id), Times.Once);
            _repository.Verify(x => x.Delete(tshirt), Times.Once);
        }

        private TshirtDeletedConsumer _target;
        private Mock<ConsumeContext<TshirtMessageDelete>> _context;
        private Mock<ITshirtRepository> _repository;

        private readonly Fixture _fixture = new Fixture();
    }
}
