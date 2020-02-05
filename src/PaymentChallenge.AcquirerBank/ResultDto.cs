namespace PaymentChallenge.AcquirerBank
{
    public struct ResultDto
    {
        public ResultDto(string status, string paymentReference)
        {
            Status = status;
            PaymentReference = paymentReference;
        }

        public string Status { get; }
        public string PaymentReference { get; }
    }


}