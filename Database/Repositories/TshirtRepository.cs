using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class TshirtRepository : ITshirtRepository
    {
        public TshirtRepository(StoreContext storeContext)
        {
            _dbSet = storeContext.Set<Tshirt>();
            _context = storeContext;
        }

        public IQueryable<Tshirt> GetAll()
        {
            return _dbSet
                .AsNoTracking()
                .Include(t => t.Reviews)
                .Include(t => t.PriceOffer);
        }

        public Tshirt? GetById(int id)
        {
            return _dbSet
                .Include(t => t.Reviews)
                .Include(t => t.PriceOffer)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(Tshirt tshirt)
        {
            _dbSet.Update(tshirt);
            _context.SaveChanges();
        }

        public void Delete(Tshirt tshirt)
        {
            _dbSet.Remove(tshirt);
            _context.SaveChanges();
        }

        public void Add(Tshirt tshirt)
        {
            _dbSet.Add(tshirt);
            _context.SaveChanges();
        }

        private DbSet<Tshirt> _dbSet;
        private StoreContext _context;
    }
}
