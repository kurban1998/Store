using Database.Models;
using Database.Repositories;
using MassTransit;
using SharedModels.Basket;

namespace ServiceLoyaltyManagement
{
    public class PurchaseCompletedConsumer : IConsumer<BasketUserMessage>
    {
        public PurchaseCompletedConsumer(ICustomerRepository loyalCustomerRepository)
        {
            _loyalCustomerRepository = loyalCustomerRepository;
        }

        public Task Consume(ConsumeContext<BasketUserMessage> context)
        {
            var basketUser = context.Message.Basket;
            UpsertUser(basketUser);
            AddBonuses(basketUser);
            return Task.CompletedTask;
        }

        private void AddBonuses(BasketUser basketUser)
        {
            var customer = _loyalCustomerRepository.GetByUserName(basketUser.UserName);
            if(customer != null)
            {
                if (basketUser.Products.Count() > MinimumQuantityGoodsForBonus)
                {
                    customer.Bonuses += Bonus;
                }

                _loyalCustomerRepository.Update(customer);
            }
        }

        private void UpsertUser(BasketUser basketUser)
        {
            var customer = _loyalCustomerRepository.GetByUserName(basketUser.UserName);
            if(customer != null)
            {
                customer.PurchaseNumber += basketUser.Products.Count();
                _loyalCustomerRepository.Update(customer);
            }
            else
            {
                _loyalCustomerRepository.Add(new Customer
                {
                    Name = basketUser.UserName,
                    Address = basketUser.Address,
                    PurchaseNumber = basketUser.Products.Count(),
                    Bonuses = 0
                });
            }
        }

        private const int MinimumQuantityGoodsForBonus = 3;
        private const double Bonus = 1000;

        private readonly ICustomerRepository _loyalCustomerRepository;
    }
}
