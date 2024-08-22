using ApiService.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using ThisrtApiService.Models;
using ThisrtApiService.Models.Reviews;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    [Route("{controller}")]
    public class HomeController : Controller
    {
        public HomeController(
            HttpClient httpClient,
            IOptions<ApiUrlOptions> options, 
            ILogger<HomeController> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        [Route("/")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var thirtsResponce = await _httpClient.GetStringAsync(_options.TshirtUrl);
            var tshirts = JsonConvert.DeserializeObject<IEnumerable<TshirtDTO>>(thirtsResponce);

            var model = new GeneralModel
            {
                TshirtsDTO = tshirts
            };

            if (User.Identity?.Name != null)
            {
                var customerResponce = await _httpClient.GetStringAsync($"{_options.CustomerUrl}/{User.Identity?.Name}");
                
                if (customerResponce != null)
                {
                    var customer = JsonConvert.DeserializeObject<CustomerDto>(customerResponce);
                    model.Customer = customer;
                }
            }

            string? successMessage = TempData["SuccessMessage"] as string;
            ViewBag.SuccessMessage = successMessage;
            
            var itemAdded = HttpContext.Session.GetString("ItemAdded");
            if (itemAdded != null)
            {
                ViewBag.ItemAdded = true;
                HttpContext.Session.Remove("ItemAdded");
            }

            return View(model);
        }

        [Route("tshirt/{tshirtid}")]
        public async Task<IActionResult> GetTshirt(int tshirtid)
        {
            var url = _options.TshirtUrl + "/tshirt/" + tshirtid;

            var responce = await _httpClient.GetStringAsync(url);

            var tshirt = JsonConvert.DeserializeObject<TshirtDTO>(responce);

            return View(tshirt);
        }

        [HttpPost("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews(int tshirtId)
        {
            var url = _options.TshirtUrl + "/reviews/" + tshirtId;

            var reviewsResponce = await _httpClient.GetStringAsync(url);

            var reviews = JsonConvert.DeserializeObject<TshirtReviewsDTO>(reviewsResponce);

            return View(reviews);
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview(
            [Required] int tshirtId,
            [StringLength(MaxVoterNameLength, ErrorMessage = "Максимальная длина имени не должна превышать 20 символов")] string votername,
            [Range(1, 5, ErrorMessage = $"Значение поля 'оценка' должно быть от 1 до 5")] int grade,
            [MaxLength(MaxCommentLength, ErrorMessage = "Максимальная длина комментария не должна превышать {1} символов")] 
            [MinLength(MinCommentLength, ErrorMessage = "Максимальная длина комментария не должна превышать {1} символов")]   string? comment)
        {
            var url = _options.TshirtUrl + "/AddReview";

            var reviewDto = new ReviewDTO
            {
                TshirtId = tshirtId,
                VoterName = votername,
                NumStats = grade,
                Comment = comment
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(reviewDto),
                Encoding.UTF8,
                Application.Json);

            using var httpResponceMessage = await _httpClient.PostAsync(url, content);

            httpResponceMessage.EnsureSuccessStatusCode();

            TempData["SuccessMessage"] = "Спасибо за отзыв!";

            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly HttpClient _httpClient;
        private readonly ApiUrlOptions _options;
        private readonly ILogger<HomeController> _logger;

        private const int MaxVoterNameLength = 20;
        private const int MaxCommentLength = 150;
        private const int MinCommentLength = 10;
    }
}
