using System;

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

        public static bool operator < (Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return left.Amount < right.Amount;
        }

        public static bool operator > (Money left, Money right)
        {
            AssertSameCurrency(left, right);
            return left.Amount > right.Amount;
        }

        public static void AssertSameCurrency(Money first, Money second)
        {
            if (first.Currency != second.Currency)
                throw new ArgumentException("Money Currency Not Equal");
        }

       
    }
}