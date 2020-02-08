namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class PaymentResponseDto
    {
        /// <summary>
        /// The status of the payment : Success or Fail
        /// </summary>
        public string PaymentStatus { get; set; }
        /// <summary>
        /// Id of the payment
        /// </summary>
        public string PaymentId { get; set; }
    }
}