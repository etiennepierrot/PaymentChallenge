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
        public static implicit operator string(MerchantId merchantId) => merchantId._id;
       
        public static bool operator == (MerchantId x, MerchantId y)
        {
            return x._id == y._id;
        }
        public static bool operator != (MerchantId x, MerchantId y) 
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            return _id.Equals(((MerchantId)obj)._id);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}