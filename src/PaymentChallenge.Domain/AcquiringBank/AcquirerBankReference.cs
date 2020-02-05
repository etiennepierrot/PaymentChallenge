namespace PaymentChallenge.Domain.AcquiringBank
{
    public struct AcquirerBankReference
    {
        private readonly string _reference;

        public AcquirerBankReference(string reference)
        {
            _reference = reference;
        }

        public static implicit operator AcquirerBankReference (string str) => new AcquirerBankReference(str);
    }
}