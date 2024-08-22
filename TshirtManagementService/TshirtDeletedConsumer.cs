using Database.Repositories;
using MassTransit;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService
{
    public class TshirtDeletedConsumer : IConsumer<TshirtMessageDelete>
    {
        public TshirtDeletedConsumer(ITshirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public Task Consume(ConsumeContext<TshirtMessageDelete> context)
        {
            var tshirt = _tshirtRepository.GetById(context.Message.Id);
            _tshirtRepository.Delete(tshirt);
            return Task.CompletedTask;
        }

        private readonly ITshirtRepository _tshirtRepository;
    }
}
