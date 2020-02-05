namespace PaymentChallenge.Domain.Payments
{
    public struct MerchantReference
    {
        private readonly string _merchantReference;

        public MerchantReference(string merchantReference)
        {
            _merchantReference = merchantReference;
        }
        
        public static implicit operator string(MerchantReference merchantReference) => merchantReference._merchantReference;
        public static implicit operator MerchantReference(string str) => new MerchantReference(str);
    }
}