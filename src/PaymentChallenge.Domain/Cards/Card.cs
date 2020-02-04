namespace PaymentChallenge.Domain.Cards
{
    public class Card
    {
        public Card(string cardNumber, string cvv, string expirationDate)
        {
            CardNumber = cardNumber;
            Cvv = cvv;
            ExpirationDate = expirationDate;
        }
        public string CardNumber { get; }
        public string Cvv { get; }
        public string ExpirationDate { get; }
    }
}