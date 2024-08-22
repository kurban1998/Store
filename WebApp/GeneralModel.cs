using ThisrtApiService.Models;

namespace WebApp
{
    public class GeneralModel
    {
        public IEnumerable<TshirtDTO> TshirtsDTO { get; set; }

        public CustomerDto Customer { get; set; }
    }
}
