namespace SharedModels.Basket
{
    public class BasketUserMessage
    {
        public BasketUser Basket { get; set; }

        public BasketUserMessage()
        {
            Basket = new BasketUser();
        }
    }
}
