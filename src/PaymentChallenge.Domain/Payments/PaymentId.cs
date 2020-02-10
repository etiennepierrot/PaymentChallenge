namespace PaymentChallenge.Domain.Payments
{
    
    public struct PaymentId
    {
        private readonly string _id;

        public PaymentId(string id)
        {
            _id = id;
        }
        
        public static implicit operator string(PaymentId id) => id._id;
        public static implicit operator PaymentId(string str) => new PaymentId(str);
    }
}