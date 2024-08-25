using AutoFixture;
using Database.Models;
using Database.Models.Helpers;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.TshirtManagemetService;
using TshirtManagementService.Builder;

namespace TshirtManagementService.UnitTests
{
    [TestClass]
    public class TshirtAddedConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _context = new Mock<ConsumeContext<TshirtMessageAdd>>();
            _builder = new Mock<ITshirtBuilder>();
            _repository = new Mock<ITshirtRepository>();
            _target = new TshirtAddedConsumer(_builder.Object, _repository.Object);
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var request = _fixture.Create<TshirtMessageAdd>();
            var tshirt = _fixture.Build<Tshirt>()
                .With(x => x.Price, request.Price)
                .With(x => x.Color, request.Color)
                .With(x => x.Brand, request.Brand)
                .With(x => x.Size, (Size)request.Size)
                .With(x => x.Gender, (Gender)request.Gender)
                .Create();

            _context.Setup(x => x.Message).Returns(request);

            _builder.Setup(x => x.Create()).Returns(_builder.Object);
            _builder.Setup(x => x.SetGender((Gender)request.Gender)).Returns(_builder.Object);
            _builder.Setup(x => x.SetBrand(request.Brand)).Returns(_builder.Object);
            _builder.Setup(x => x.SetPrice(request.Price)).Returns(_builder.Object);
            _builder.Setup(x => x.SetImageName(request.ImageName)).Returns(_builder.Object);
            _builder.Setup(x => x.SetSize((Size)request.Size)).Returns(_builder.Object);
            _builder.Setup(x => x.SetColor(request.Color)).Returns(_builder.Object);
            _builder.Setup(x => x.Build()).Returns(tshirt);

            _repository.Setup(x => x.Add(tshirt));

            // act
            _target.Consume(_context.Object);

            // assert
            _context.Verify(x => x.Message, Times.Once);

            _builder.Verify(x => x.Create(), Times.Once);
            _builder.Verify(x => x.SetGender((Gender)request.Gender), Times.Once);
            _builder.Verify(x => x.SetBrand(request.Brand), Times.Once);
            _builder.Verify(x => x.SetPrice(request.Price), Times.Once);
            _builder.Verify(x => x.SetSize((Size)request.Size), Times.Once);
            _builder.Verify(x => x.SetImageName(request.ImageName), Times.Once);
            _builder.Verify(x => x.SetColor(request.Color), Times.Once);
            _builder.Verify(x => x.Build(), Times.Once);

            _repository.Verify(x => x.Add(tshirt), Times.Once);
        }

        private TshirtAddedConsumer _target;
        private Mock<ITshirtRepository> _repository;
        private Mock<ITshirtBuilder> _builder;
        private Mock<ConsumeContext<TshirtMessageAdd>> _context;
        
        private readonly Fixture _fixture = new Fixture();
    }
}
