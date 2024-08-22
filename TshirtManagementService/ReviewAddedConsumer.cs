using Database.Models;
using Database.Repositories;
using MassTransit;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService
{
    public sealed class ReviewAddedConsumer : IConsumer<AddReviewMessage>
    {
        public ReviewAddedConsumer(ITshirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public Task Consume(ConsumeContext<AddReviewMessage> context)
        {
            var message = context.Message;
            var tshirt = _tshirtRepository.GetById(message.TshirtId);

            var review = new Review
            {
                Comment = message.Comment,
                NumStats = message.NumStats,
                VoterName = message.VoterName,
            };

            tshirt.Reviews.Add(review);
            _tshirtRepository.Update(tshirt);
            return Task.CompletedTask;
        }

        private readonly ITshirtRepository _tshirtRepository;
    }
}
