namespace Database.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int PurchaseNumber { get; set; }

        public double Bonuses { get; set; } = 0;
    }
}
