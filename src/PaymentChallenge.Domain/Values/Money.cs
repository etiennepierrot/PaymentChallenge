namespace PaymentChallenge.Domain.Values
{
    public struct Money
    {
        public int Amount { get; }
        public Currency Currency { get; }

        public Money(int amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}