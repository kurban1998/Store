using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Basket;

namespace ThisrtApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        public OrderController(
            IAuthorizationService authorizationService,
            IBus bus) 
        {
            _authorizationService = authorizationService;
            _bus = bus;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task CreateOrder(BasketUser basketUser)
        {
            var message = new BasketUserMessage
            {
                Basket = basketUser
            };

            await _bus.Publish<BasketUserMessage>(message); 
        }

        private readonly IAuthorizationService _authorizationService;
        private readonly IBus _bus;
    }
}
