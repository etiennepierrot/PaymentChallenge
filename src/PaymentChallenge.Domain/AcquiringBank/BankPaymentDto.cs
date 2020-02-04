namespace PaymentChallenge.Domain.AcquiringBank
{
    public struct BankPaymentDto
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpirationDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}