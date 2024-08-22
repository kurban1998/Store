using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository(StoreContext storeContext) 
        {
            _dbSet = storeContext.Set<Customer>();
            _context = storeContext;
        }

        public Customer GetByUserName(string userName)
        {
            return _dbSet.FirstOrDefault(x => x.Name == userName);
        }

        public IQueryable<Customer> GetAll()
        {
            return _dbSet;
        }

        public void Update(Customer customer)
        {
            _dbSet.Update(customer);
            _context.SaveChanges();
        }

        public void Add(Customer customer)
        {
            _dbSet.Add(customer);
            _context.SaveChanges();
        }

        private DbSet<Customer> _dbSet;
        private StoreContext _context;
    }
}
