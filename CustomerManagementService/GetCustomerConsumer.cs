using Database.Repositories;
using MassTransit;
using SharedModels.CustomerService;

namespace CustomerManagementService
{
    public sealed class GetCustomerConsumer : IConsumer<CustomerRequest>
    {
        public GetCustomerConsumer(ICustomerRepository customerRepository) 
        {
            _customerRepository = customerRepository;
        }

        public Task Consume(ConsumeContext<CustomerRequest> context)
        {
            var customer = _customerRepository.GetByUserName(context.Message.UserName);
            return context.RespondAsync<CustomerResponce>(new CustomerResponce { Customer = customer });
        }

        private readonly ICustomerRepository _customerRepository;
    }
}
