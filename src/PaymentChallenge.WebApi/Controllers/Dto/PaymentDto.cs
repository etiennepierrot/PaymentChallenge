namespace PaymentChallenge.WebApi.Controllers.Dto
{
    
    
    public class PaymentDto
    {
        public CardDto Card { get; set;  }
        public MoneyDto Amount { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public string MerchantReference { get; set; }
        //Remark : the bank reference is not included in the API interface, i think it should be hidden of the client
    }
}