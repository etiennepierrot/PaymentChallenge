namespace PaymentChallenge.Domain.Merchants
{
    public class Merchant
    {
        public MerchantId Id { get; }

        public Merchant(MerchantId id)
        {
            Id = id;
        }
    }
}