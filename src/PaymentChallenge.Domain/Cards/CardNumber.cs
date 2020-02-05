using System;
using System.Text.RegularExpressions;

namespace PaymentChallenge.Domain.Cards
{
    public struct CardNumber
    {
        private readonly string _cardNumber;

        public CardNumber(string cardNumber)
        {
            _cardNumber = cardNumber;
        }


        public string UnMasked => _cardNumber;

        public string Masked
        {
            get
            {
                var firstDigits = _cardNumber.Substring(0, 4);
                var lastDigits = _cardNumber.Substring(_cardNumber.Length - 4, 4);
                var requiredMask = new string('X', _cardNumber.Length - firstDigits.Length - lastDigits.Length);
                var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
                return  Regex.Replace(maskedString, ".{4}", "$0 ").TrimEnd();
            }
        }

        public static implicit operator string(CardNumber merchantReference) => merchantReference.Masked;
        public static implicit operator CardNumber(string str) => new CardNumber(str);
    }
}