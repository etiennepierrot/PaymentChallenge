namespace PaymentChallenge.Domain.Merchants
{
    public struct MerchantId
    {
        private readonly string _id;

        public MerchantId(string id)
        {
            _id = id;
        }
        
        public static implicit operator MerchantId(string str) => new MerchantId(str);
    }
}