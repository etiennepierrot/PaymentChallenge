using System;
using PaymentChallenge.Domain;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.WebApi
{
    public class PaymentIdGenerator : IdGenerator
    {
        public PaymentId GeneratePaymentId()
        {
            return new PaymentId(Guid.NewGuid().ToString());
        }
    }
}