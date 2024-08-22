namespace Database.Models
{
    public sealed class PriceOffer
    {
        public int PriceOfferId { get; init; }

        public decimal NewPrice { get; init; }

        public string? PromotionalText { get; init; }

        // Dependencies
        public int TshirtId { get; init; }

        public Tshirt Tshirt { get; init; } = null!;
    }
}
