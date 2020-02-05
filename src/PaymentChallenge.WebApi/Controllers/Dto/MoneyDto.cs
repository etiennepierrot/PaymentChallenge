using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class MoneyDto
    {
        public int Amount { get; set; }
        public Currency Currency { get; set; }
    }
}