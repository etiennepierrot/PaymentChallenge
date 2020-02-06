namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class PaymentRequestDto
    {
        /// <summary>
        /// The card use for the payment
        /// </summary>
        public CardDto Card { get; set; }
        //TODO this should be get from basic auth
        public string  MerchantId { get; set; }
        /// <summary>
        /// The amount to charge
        /// </summary>
        public MoneyDto AmountToCharge { get; set; }
        public string MerchantReference { get; set; }
    }
}