using Database.Repositories;
using MassTransit;
using SharedModels.TshirtService;

namespace TshirtManagementService
{
    public class TshirtsDeletedConsumer : IConsumer<TshirtsMessageDelete>
    {
        public TshirtsDeletedConsumer(ITshirtRepository tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        public Task Consume(ConsumeContext<TshirtsMessageDelete> context)
        {
            var tshirtIds = context.Message.TshirtIds;

            foreach(var tshirtId in tshirtIds)
            {
                var tshirtDeleted = _tshirtRepository.GetById(tshirtId);
                _tshirtRepository.Delete(tshirtDeleted);
            }

            return Task.CompletedTask;
        }

        private readonly ITshirtRepository _tshirtRepository;
    }
}
