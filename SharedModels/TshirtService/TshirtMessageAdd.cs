using SharedModels.Enums;

namespace SharedModels.TshirtManagemetService
{
    public class TshirtMessageAdd
    {
        public int Id { get; init; }

        public string Brand { get; init; }

        public string Color { get; init; }

        public decimal Price { get; init; }

        public string ImageName { get; init; }

        public Size Size { get; init; }

        public Gender Gender { get; init; }
    }
}
