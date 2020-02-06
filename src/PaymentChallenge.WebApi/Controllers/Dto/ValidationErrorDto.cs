using System.Collections.Generic;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class ValidationErrorDto
    {
        public List<ErrorDto> Errors { get; set; }
    }
}