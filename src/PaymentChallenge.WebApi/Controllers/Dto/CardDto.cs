namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class CardDto
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpirationDate { get; set; }
    }
}