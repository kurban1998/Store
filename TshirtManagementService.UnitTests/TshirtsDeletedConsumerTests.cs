using AutoFixture;
using Database.Models;
using Database.Repositories;
using MassTransit;
using Moq;
using SharedModels.TshirtService;

namespace TshirtManagementService.UnitTests
{
    [TestClass]
    public class TshirtsDeletedConsumerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _context = new Mock<ConsumeContext<TshirtsMessageDelete>>(MockBehavior.Strict);
            _repository = new Mock<ITshirtRepository>(MockBehavior.Strict);
            _target = new TshirtsDeletedConsumer(_repository.Object);
        }

        [TestMethod]
        public void Consume()
        {
            // arrange
            var tshirts = _fixture.CreateMany<Tshirt>().ToList();
            var tshirtsId = new int[tshirts.Count()];
            for (int i = 0; i < tshirtsId.Length; i++)
            {
                tshirtsId[i] = tshirts[i].Id;
            }


            var request = _fixture.Build<TshirtsMessageDelete>()
                .With(x => x.TshirtIds, tshirtsId)
                .Create();

            _context.Setup(x => x.Message).Returns(request);

            int j = 0;
            foreach(var id in tshirtsId)
            {
                _repository.Setup(x => x.GetById(id)).Returns(tshirts[j]);
                _repository.Setup(x => x.Delete(tshirts[j]));
                j++;
            }
            
            // act
            _target.Consume(_context.Object);

            // assert

            int k = 0;
            foreach (var id in tshirtsId)
            {
                _repository.Verify(x => x.GetById(id), Times.Once);
                _repository.Verify(x => x.Delete(tshirts[k]), Times.Once);
                k++;
            }
        }

        private TshirtsDeletedConsumer _target;
        private Mock<ITshirtRepository> _repository;
        private Mock<ConsumeContext<TshirtsMessageDelete>> _context;

        private readonly Fixture _fixture = new Fixture();
    }
}
