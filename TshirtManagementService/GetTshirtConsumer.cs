using Database.Models;
using Database.Repositories;
using MassTransit;
using Newtonsoft.Json;
using SharedModels.TshirtManagemetService;

namespace TshirtManagementService
{
    public class GetTshirtConsumer : IConsumer<GetTshirtRequest>
    {
        public GetTshirtConsumer(ITshirtRepository tshirtRepository) 
        {
            _tshirtRepository = tshirtRepository;
        }

        public Task Consume(ConsumeContext<GetTshirtRequest> context)
        {
            var tshirt = _tshirtRepository.GetById(context.Message.TshirtId);

            return context.RespondAsync<GetTshirtResponce>(new GetTshirtResponce 
            { 
                Tshirt = tshirt,
                Reviews = tshirt.Reviews.ToList()
            });
        }

        private readonly ITshirtRepository _tshirtRepository;
    }
}
