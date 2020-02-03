using PaymentChallenge.Domain.Merchants;

namespace PaymentChallenge.Domain.Shoppers
{
    public struct RegisterChopper
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public MerchantId  MerchantId { get; set; }
    }
}