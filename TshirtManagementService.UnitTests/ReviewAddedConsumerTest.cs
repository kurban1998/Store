using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService.UnitTests
{
    [TestClass]
    public class ReviewAddedConsumerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<ConsumeContext<AddReviewMessage>>(MockBehavior.Strict);
            _repository = new Mock<ITshirtRepository>(MockBehavior.Strict);
            _target = new ReviewAddedConsumer(_repository.Object);
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var id = _fixture.Create<int>();
            var tshirt = _fixture.Build<Tshirt>()
                .With(x => x.Id, id)
                .Create();

            var request = _fixture.Build<AddReviewMessage>()
                .With(x => x.TshirtId, tshirt.Id)
                .Create();

            _context.Setup(x => x.Message).Returns(request);
            _repository.Setup(x => x.GetById(id)).Returns(tshirt);
            _repository.Setup(x => x.Update(tshirt));

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.Update(tshirt), Times.Once);
            _repository.Verify(x => x.GetById(id), Times.Once);
        }

        private ReviewAddedConsumer _target;
        private Mock<ITshirtRepository> _repository;
        private Mock<ConsumeContext<AddReviewMessage>> _context;

        private readonly Fixture _fixture = new Fixture();
    }
}
