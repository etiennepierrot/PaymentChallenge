using System.Linq;
using System.Text.RegularExpressions;

namespace PaymentChallenge.Domain.Cards
{
    public class CardNumber
    {
        private readonly string _cardNumber;

        private CardNumber(string cardNumber)
        {
            _cardNumber = cardNumber;
        }
        

        private string Masked
        {
            get
            {
                var firstDigits = _cardNumber.Substring(0, 4);
                var lastDigits = _cardNumber.Substring(_cardNumber.Length - 4, 4);
                var requiredMask = new string('X', _cardNumber.Length - firstDigits.Length - lastDigits.Length);
                var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
                return Regex.Replace(maskedString, ".{4}", "$0 ").TrimEnd();
            }
        }

        public static implicit operator string(CardNumber merchantReference) => merchantReference.Masked;
        public static implicit operator CardNumber(string str) => new CardNumber(str);

        public bool IsValid()
        {
            //steal from https://github.com/JeremySkinner/FluentValidation/blob/master/src/FluentValidation/Validators/CreditCardValidator.cs
            string value = _cardNumber
                .Replace("-", "")
                .Replace(" ", "");

            
            int checksum = 0;
            bool evenDigit = false;
            foreach (char digit in value.ToCharArray().Reverse()) {
                if (!char.IsDigit(digit)) {
                    return false;
                }

                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;

                while (digitValue > 0) {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }

            return (checksum % 10) == 0;
        }

        /// <summary>
        public string GetUnMaskerCardNumber()
        {
            return _cardNumber;
        }
    }
}