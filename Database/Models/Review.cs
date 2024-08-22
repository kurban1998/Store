namespace Database.Models
{
    public sealed class Review
    {
        public int ReviewId { get; init; }

        public string VoterName { get; init; }

        public int NumStats { get; init; }

        public string? Comment { get; init; }

        // Dependencies
        public int TshirtId { get; init; }

        public Tshirt? Tshirt { get; init; } = null!;
    }
}
