using Database.Models.Helpers;
using Database.Repositories;
using MassTransit;
using SharedModels.TshirtManagemetService;
using TshirtManagementService.Builder;

namespace TshirtManagementService
{
    public class TshirtAddedConsumer : IConsumer<TshirtMessageAdd>
    {
        public TshirtAddedConsumer(
            ITshirtBuilder tshirtBuilder,
            ITshirtRepository tshirtRepository)
        {
            _tshirtBuilder = tshirtBuilder;
            _tshirtRepository = tshirtRepository;
        }

        public Task Consume(ConsumeContext<TshirtMessageAdd> context)
        {
            var tshirtMessage = context.Message;

            var newTshirt = _tshirtBuilder
                .Create()
                .SetBrand(tshirtMessage.Brand)
                .SetGender((Gender)tshirtMessage.Gender)
                .SetColor(tshirtMessage.Color)
                .SetImageName(tshirtMessage.ImageName)
                .SetPrice(tshirtMessage.Price)
                .SetSize((Size)tshirtMessage.Size)
                .Build();

            _tshirtRepository.Add(newTshirt);
            return Task.CompletedTask;
        }

        private readonly ITshirtBuilder _tshirtBuilder;
        private readonly ITshirtRepository _tshirtRepository;
    }
}
