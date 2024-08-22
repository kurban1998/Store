namespace SharedModels.Basket
{
    public class BasketUser
    {
        public Guid BasketId { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
