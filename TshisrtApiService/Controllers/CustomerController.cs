using Database.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels.CustomerService;

namespace ThisrtApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        public CustomerController(
            IBus bus) 
        {
            _bus = bus;
        }

        [HttpGet("{userName}")]
        public async Task<Customer> GetUserByName(string userName)
        {
            var responce = await _bus.Request<CustomerRequest, CustomerResponce>(userName);
            return responce.Message.Customer;
        }

        private readonly IBus _bus;
    }
}
