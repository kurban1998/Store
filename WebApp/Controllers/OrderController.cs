using ApiService.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedModels.Basket;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    [Route("{controller}")]
    public class OrderController : Controller
    {
        public OrderController(
            HttpClient httpClient,
            IOptions<ApiUrlOptions> options,
            ILogger<OrderController> logger) 
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(string address)
        {
            var basketUser = HttpContext.Session.GetObjectFromJson<BasketUser>("Basket");
            basketUser.Address = address;

            var content = new StringContent(
               JsonConvert.SerializeObject(basketUser),
               Encoding.UTF8,
               Application.Json);

            var response = await _httpClient.PostAsync(_options.OrderUrl + "/Create", content);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Clear();
                await DeleteTshirtsHaveBeenOrdered(basketUser);
                return View();
            }
            else
            {
                return BadRequest();
            }  
        }

        private async Task DeleteTshirtsHaveBeenOrdered(BasketUser basketUser)
        {
            var tshirtIds = basketUser.Products.Select(x => x.TshirtId);

            var content = new StringContent(
              JsonConvert.SerializeObject(tshirtIds),
              Encoding.UTF8,
              Application.Json);

            var responce = await _httpClient.PostAsync(_options.TshirtUrl + "/DeleteTshirtsByIds", content);
            if (responce.IsSuccessStatusCode) { return; }
            else throw new Exception("Error");
        }

        private readonly HttpClient _httpClient;
        private readonly ApiUrlOptions _options;
        private readonly ILogger<OrderController> _logger;
    }
}
