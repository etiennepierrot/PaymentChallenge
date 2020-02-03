namespace PaymentChallenge.Domain.Shoppers
{
    public interface ShopperRepository
    {
        Shopper Get(ShopperId shopperId);
        void Save(Shopper shopper);
    }
}