namespace PaymentChallenge.Domain.Payments
{
    public struct PaymentResponse
    {
        public PaymentStatus PaymentStatus { get; }
        public PaymentId PaymentId { get; }

        public PaymentResponse(PaymentStatus paymentStatus, PaymentId paymentId)
        {
            PaymentStatus = paymentStatus;
            PaymentId = paymentId;
        }
    }
}