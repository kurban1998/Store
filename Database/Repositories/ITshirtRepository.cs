using Database.Models;

namespace Database.Repositories
{
    public interface ITshirtRepository
    {
        public IQueryable<Tshirt> GetAll();

        public Tshirt GetById(int id);

        public void Update(Tshirt tshirt);

        public void Delete(Tshirt tshirt);

        public void Add(Tshirt tshirt);
    }
}
