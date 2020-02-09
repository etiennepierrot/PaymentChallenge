using PaymentChallenge.Domain.Cards;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public static class CardExtensions
    {
        public static CardDto ToDto(this Card card)
        {
            return new CardDto
            {
                CardNumber = card.CardNumber,
                Cvv = card.Cvv,
                ExpirationDate = card.ExpirationDate
            };
        }
    }
}