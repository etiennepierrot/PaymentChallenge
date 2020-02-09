using System;
using System.Linq;
using FluentValidation.Results;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.Domain.Values;
using PaymentChallenge.WebApi.Controllers.Dto;

namespace PaymentChallenge.WebApi.Controllers
{
    public class DtoConverter
    {
        public static PaymentRequest CreateCommand(PaymentRequestDto paymentRequest, MerchantId merchantId)
        {
            return new PaymentRequest(DtoToModel(paymentRequest),
                merchantId, DtoToModel(paymentRequest.AmountToCharge), 
                paymentRequest.MerchantReference);
        }

        public static ValidationErrorDto ToDto(ValidationResult validationResult)
        {
            return new ValidationErrorDto()
            {
                Errors = validationResult.Errors.Select(x => new ErrorDto
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage
                }).ToList()
            };
        }

        private static Money DtoToModel(MoneyDto moneyDto)
        {
            return new Money(moneyDto.Amount,Enum.Parse<Currency>(moneyDto.Currency, true) );
        }

        private static Card DtoToModel(PaymentRequestDto paymentRequest)
        {
            return new Card(paymentRequest.Card.CardNumber, paymentRequest.Card.Cvv, paymentRequest.Card.ExpirationDate);
        }
    }
}