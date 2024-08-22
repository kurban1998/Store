using Database.Models.Helpers;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public PriceOffer? PriceOffer { get; init; }

        [JsonIgnore]
        public ICollection<Review> Reviews = new List<Review>();
    }
}
