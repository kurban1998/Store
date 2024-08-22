using MassTransit;
using SharedModels.TshirtManagemetService;
using ThisrtApiService.Models;

namespace ThisrtApiService
{
    public class TshirtCacheUpdaterService : BackgroundService
    {
        public TshirtCacheUpdaterService(
            IBus bus,
            RedisService redisService,
            IServiceProvider serviceProvider)
        {
            _bus = bus;
            _redisService = redisService;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(UpdateCache, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void UpdateCache(object state)
        {
            try
            {
                var responce = await _bus.Request<TshirtMessageGetAllRequest, TshirtMessageGetAllResponce>(new TshirtMessageGetAllRequest());
                var tshirtsDto = responce.Message.Tshirts.AsQueryable().MapTshirtsToDto();
                foreach (var item in tshirtsDto)
                {
                    _redisService.SaveTshirt(item);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            #region Через базу напрямую
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var client = scope.ServiceProvider.GetRequiredService<ITshirtRepository>();

            //    var tshirtsDto = client.GetAll().MapTshirtsToDto();

            //    foreach (var item in tshirtsDto)
            //    {
            //        _redisService.SaveTshirt(item);
            //    }
            //}
            #endregion
        }

        private readonly IBus _bus;
        private readonly RedisService _redisService;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
    }
}