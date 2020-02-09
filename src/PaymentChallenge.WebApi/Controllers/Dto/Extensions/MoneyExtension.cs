using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public static class MoneyExtension
    {
        public static MoneyDto ToDto(this Money money)
        {
            return new MoneyDto
            {
                Amount = money.Amount,
                Currency = money.Currency.ToString()
            };
        }
    }
}