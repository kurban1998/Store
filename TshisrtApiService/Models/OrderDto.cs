namespace ThisrtApiService.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string UserName { get; init; }

        public int TshirtId { get; init; }

        public string Address { get; init; }

        public decimal Price { get; set; }
    }
}
