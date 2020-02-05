namespace PaymentChallenge.AcquirerBank
{
    internal struct BankPaymentDto
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpirationDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
    }
}