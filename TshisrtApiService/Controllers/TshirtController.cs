using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels.TshirtManagemetService;
using SharedModels.TshirtService;
using ThisrtApiService.Models;
using ThisrtApiService.Models.Reviews;

namespace ThisrtApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TshirtController : ControllerBase
    {
        // https://localhost:7038
        public TshirtController(
            RedisService redisService,
            IBus bus) 
        {
            _bus = bus;
            _redisService = redisService;
        }

        // ../Tshirt
        [HttpGet]
        public IList<TshirtDTO> GetTshirts()
        {
            return _redisService.GetTshirts();
        }

        [HttpGet("tshirt/{tshirtId}")]
        public TshirtDTO GetTshirtById(int tshirtId)
        {
            return _redisService.GetTshirt(tshirtId);
        }

        [HttpPost("AddReview")]
        public async Task AddReviewToTshirt(ReviewDTO reviewDTO)
        {
            var review = new AddReviewMessage
            {
                TshirtId = reviewDTO.TshirtId,
                NumStats = (int)reviewDTO.NumStats,
                Comment = reviewDTO.Comment,
                VoterName = reviewDTO.VoterName
            };

            await _bus.Publish<AddReviewMessage>(review, ctx => ctx.SetRoutingKey("AddReviewMessage"));
        }

        [HttpPost("DeleteTshirtById")]
        public void DeleteTshirtById(int tshirtId)
        {
            _redisService.DeleteByid(tshirtId);
            var message = new TshirtMessageDelete
            {
                Id = tshirtId
            };

            _bus.Publish<TshirtMessageDelete>(message, ctx => ctx.SetRoutingKey("TshirtDeletedConsumer"));
        }

        [HttpPost("DeleteTshirtsByIds")]
        public void DeleteTshirtsByIds(int[] tshirtIds)
        {
            foreach (int id in tshirtIds)
            {
                _redisService.DeleteByid(id);
            }
            
            var message = new TshirtsMessageDelete
            {
                TshirtIds = tshirtIds
            };

            _bus.Publish<TshirtsMessageDelete>(message, ctx => ctx.SetRoutingKey("TshirtsMessageDelete"));
        }

        [HttpGet("reviews/{tshirtId}")]
        public async Task<TshirtReviewsDTO> GetReviews(int tshirtId)
        {
            var request = new GetTshirtRequest
            {
                TshirtId = tshirtId
            };

            var responce = await _bus.Request<GetTshirtRequest, GetTshirtResponce>(request);
            var tshirt = responce.Message.Tshirt;
            var reviews = responce.Message.Reviews;

            return new TshirtReviewsDTO
            {
                Brand = tshirt.Brand,
                ReviewContainer = reviews.Select(review =>
                    new GradesNamesContainer
                    {
                        VoterName = review.VoterName,
                        NumStat = review.NumStats,
                        Comment = review.Comment
                    }).ToList()
            };
        }

        private readonly IBus _bus;
        private readonly RedisService _redisService;
    }
}
