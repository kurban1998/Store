using Database.Repositories;
using MassTransit;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService
{
    public class TshirtGetAllConsumer : IConsumer<TshirtMessageGetAllRequest>
    {
        public TshirtGetAllConsumer(ITshirtRepository tshirtRepository) 
        {
            _tshirtRepository = tshirtRepository;
        }

        public Task Consume(ConsumeContext<TshirtMessageGetAllRequest> context)
        {
            var tshirts = _tshirtRepository.GetAll().ToList();
            return context.RespondAsync<TshirtMessageGetAllResponce>(new TshirtMessageGetAllResponce { Tshirts = tshirts });
        }

        private readonly ITshirtRepository _tshirtRepository;
    }
}
