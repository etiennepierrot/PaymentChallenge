using System;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class MoneyDto
    {
        /// <summary>
        /// the amount to charge in cents
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// the currency (only euros is supported (EUR) ISO 4217 (https://fr.wikipedia.org/wiki/ISO_4217)
        /// </summary>
        public string Currency { get; set; }
        
    }
}