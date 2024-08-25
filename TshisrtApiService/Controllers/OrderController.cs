using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Basket;

namespace ThisrtApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        public OrderController(
            IBus bus) 
        {
            _bus = bus;
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
    }
}
