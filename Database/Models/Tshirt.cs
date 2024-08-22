using Database.Models.Helpers;

namespace Database.Models
{
    public sealed class Tshirt
    {
        public int Id { get; init; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }

        public string ImageName { get; set; }

        public Size Size { get; set; }

        public Gender Gender { get; set; }

        //Dependencies
        public PriceOffer? PriceOffer { get; init; }

        public ICollection<Review> Reviews = new List<Review>();
    }
}
