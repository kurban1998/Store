namespace SharedModels.TshirtManagemetService
{
    public sealed class AddReviewMessage
    {
        public int TshirtId { get; init; }

        public string VoterName { get; init; }

        public int NumStats { get; init; }

        public string? Comment { get; init; }
    }
}
