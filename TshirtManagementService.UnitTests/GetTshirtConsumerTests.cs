using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService.UnitTests
{
    [TestClass]
    public class GetTshirtConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<ConsumeContext<GetTshirtRequest>>();
            
            _repository = new Mock<ITshirtRepository>();
            _target = new GetTshirtConsumer(_repository.Object);
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [TestMethod]
        public void ConsumeTest()
        {
            // arrange
            var tshirtId = _fixture.Create<int>();
            var tshirt = _fixture.Build<Tshirt>()
                .With(x => x.Id, tshirtId)
                .WithAutoProperties()
                .Create();

            var request = _fixture.Build<GetTshirtRequest>()
                .With(x => x.TshirtId, tshirt.Id)
                .Create();

            _repository.Setup(x => x.GetById(tshirtId)).Returns(tshirt);
            _context.Setup(x => x.Message).Returns(request);

            // act
            _target.Consume(_context.Object);

            // assert
            _repository.Verify(x => x.GetById(tshirtId), Times.Once);
        }


        private GetTshirtConsumer _target;
        private Mock<ConsumeContext<GetTshirtRequest>> _context;
        private Mock<ITshirtRepository> _repository;

        private readonly Fixture _fixture = new Fixture();
    }
}
