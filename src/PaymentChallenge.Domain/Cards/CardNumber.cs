namespace PaymentChallenge.Domain.Cards
{
    public struct CardNumber
    {
        private readonly string _cardNumber;

        public CardNumber(string cardNumber)
        {
            _cardNumber = cardNumber;
        }

        public static implicit operator string(CardNumber merchantReference) => merchantReference._cardNumber;
        public static implicit operator CardNumber(string str) => new CardNumber(str);
    }
}