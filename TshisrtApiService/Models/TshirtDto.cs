using Database.Models.Helpers;

namespace ThisrtApiService.Models
{
    public sealed class TshirtDTO
    {
        public int TshirtId { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public Size Size { get; set; }

        public decimal Price { get; set; }

        public decimal NewPrice { get; set; }

        public string ImageName { get; set; }

        public Gender Gender { get; set; }

        public string VoterNames { get; set; }

        public double NumStats { get; set; }

        public string? PromotionalText { get; set; }
    }
}