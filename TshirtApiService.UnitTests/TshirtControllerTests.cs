using AutoFixture;
using MassTransit;
using Moq;
using ThisrtApiService.Controllers;
using ThisrtApiService.Models;
using TshisrtApiService;

namespace TshirtApiService.UnitTests
{
    [TestClass]
    public class TshirtControllerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            _bus = new Mock<IBus>();
            _redis = new Mock<IRedisService>();
            _target = new TshirtController(_redis.Object, _bus.Object);
        }

        [TestMethod]
        public void GetTshirts()
        {
            // arrange 
            var tshirts = _fixture.CreateMany<TshirtDTO>().ToList();

            _redis.Setup(x => x.GetTshirts()).Returns(tshirts); 

            // act
            _target.GetTshirts();

            // assert
            _redis.Verify(x => x.GetTshirts(), Times.Once);
        }

        [TestMethod]
        public void GetTshirtsById()
        {
            // arrange
            var tshirtId = _fixture.Create<int>();
            
            var tshirt = _fixture.Build<TshirtDTO>()
                .With(x => x.TshirtId, tshirtId)
                .Create();

            _redis.Setup(x => x.GetTshirt(tshirtId)).Returns(tshirt);
            
            // act 
            _target.GetTshirtById(tshirtId);

            // assert
            _redis.Verify(x => x.GetTshirt(tshirtId), Times.Once);
        }

        private TshirtController _target;
        private Mock<IBus> _bus;
        private Mock<IRedisService> _redis;

        private readonly Fixture _fixture = new Fixture();
    }
}
