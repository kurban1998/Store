using Microsoft.AspNetCore.Mvc;
using SharedModels.Basket;

namespace WebApp.Controllers
{
    [Route("{controller}")]
    public class BasketController : Controller
    {
        public BasketController(
            ILogger<BasketController> logger)
        {
            _logger = logger;
        }

        [Route("Basket")]
        public IActionResult Basket()
        {
            var basketUser = HttpContext.Session.GetObjectFromJson<BasketUser>("Basket") ?? new BasketUser();
            return View(basketUser);
        }

        [Route("AddItemToBasket")]
        public IActionResult AddItemToBasket(string brand,
            decimal price,
            int tshirtId,
            string userName)
        {
            var basketUser = HttpContext.Session.GetObjectFromJson<BasketUser>("Basket") ?? new BasketUser();
            var product = new Product
            {
                TshirtId = tshirtId,
                Price = price,
                Brand = brand,
            };

            if (!basketUser.Products.Any(x => x.TshirtId == product.TshirtId))
            {
                basketUser.UserName = userName;
                basketUser.Products.Add(product);

                HttpContext.Session.SetObjectAsJson("Basket", basketUser);

                return Redirect("/");
            }

            return Redirect("/");
        }

        [Route("DeleteItemFromBasket")]
        public IActionResult DeleteItemFromBasket(int tshirtId)
        {
            var basketUser = HttpContext.Session.GetObjectFromJson<BasketUser>("Basket");

            basketUser.Products = basketUser.Products.Where(x => x.TshirtId != tshirtId).ToList();

            HttpContext.Session.SetObjectAsJson("Basket", basketUser);

            return Json(new { success = true });
        }

        private readonly ILogger<BasketController> _logger;
    }
}
