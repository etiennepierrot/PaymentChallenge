using PaymentChallenge.Domain.Merchants;

namespace PaymentChallenge.Domain.Shoppers
{
    public class Shopper
    {
        public Shopper(ShopperId shopperId, string firstName, string lastName, MerchantId merchantId)
        {
            Id = shopperId;
            FirstName = firstName;
            LastName = lastName;
            MerchantId = merchantId;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public MerchantId MerchantId { get;  }
        public ShopperId Id { get; }
    }
}