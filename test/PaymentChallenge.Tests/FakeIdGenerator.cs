using PaymentChallenge.Domain;
using PaymentChallenge.Domain.Shoppers;

namespace PaymentChallenge.Tests
{
    public class FakeIdGenerator : IdGenerator
    {
        public ShopperId GenerateShopperId()
        {
            return new ShopperId("FakeShopperId");
        }
    }
}