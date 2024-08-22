using Database.Models;

namespace Database.Repositories
{
    public interface ICustomerRepository
    {
        public Customer GetByUserName(string userName);

        public void Update(Customer customer);

        public IQueryable<Customer> GetAll();

        public void Add(Customer customer);
    }
}
