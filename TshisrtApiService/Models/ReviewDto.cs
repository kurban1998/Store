namespace ThisrtApiService.Models
{
    public sealed class ReviewDTO
    {
        public string VoterName { get; set; }

        public double NumStats { get; set; }

        public string? Comment { get; set; }

        public int TshirtId { get; set; }
    }
}