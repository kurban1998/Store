namespace ThisrtApiService.Models.Reviews
{
    public sealed class TshirtReviewsDTO
    {
        public string Brand { get; set; }

        public ICollection<GradesNamesContainer> ReviewContainer { get; set; } = new List<GradesNamesContainer>();
    }
}