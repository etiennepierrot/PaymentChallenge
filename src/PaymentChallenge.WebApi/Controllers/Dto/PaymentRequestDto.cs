namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class PaymentRequestDto
    {
        public CardDto Card { get; set; }
        //TODO this should be get from basic auth
        public string  MerchantId { get; set; }
        public MoneyDto AmountToCharge { get; set; }
        public string MerchantReference { get; set; }
    }
}