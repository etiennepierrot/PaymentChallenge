using PaymentChallenge.Domain.Shoppers;

namespace PaymentChallenge.Domain
{
    public interface IdGenerator
    {
        ShopperId GenerateShopperId();
    }
}