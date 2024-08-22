using Database.Models;

namespace SharedModels.TshirtManagemetService
{
    public sealed class GetTshirtResponce
    {
        public Tshirt Tshirt { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
