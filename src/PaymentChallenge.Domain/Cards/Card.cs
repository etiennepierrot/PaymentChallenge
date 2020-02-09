namespace PaymentChallenge.Domain.Cards
{
    public class Card
    {
        public Card(CardNumber cardNumber, string cvv, string expirationDate)
        {
            CardNumber = cardNumber;
            Cvv = cvv;
            ExpirationDate = expirationDate;
        }
        public CardNumber CardNumber { get; }
        public string Cvv { get; }
        public string ExpirationDate { get; }
    }
}