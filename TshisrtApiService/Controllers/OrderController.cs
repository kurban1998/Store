using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Basket;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        public OrderController(
            IBus bus,
            ILogger<OrderController> logger) 
        {
            _bus = bus;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task CreateOrder(BasketUser basketUser)
        {
            var message = new BasketUserMessage
            {
                Basket = basketUser
            };

            await _bus.Publish<BasketUserMessage>(message); 
        }

        private readonly IBus _bus;
        private readonly ILogger<OrderController> _logger;
    }
}
