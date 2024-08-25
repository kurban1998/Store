using ThisrtApiService.Models;

namespace TshisrtApiService
{
    public interface IRedisService
    {
        public void SaveTshirt(TshirtDTO tshirt);

        public TshirtDTO GetTshirt(int id);

        public IList<TshirtDTO> GetTshirts();

        public void DeleteByid(int tshirtId);
    }
}
