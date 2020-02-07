using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.WebApi.Controllers.Dto;

namespace PaymentChallenge.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly PaymentGateway _paymentGateway;
        private readonly PaymentRepository _paymentRepository;

        public PaymentsController(ILogger<PaymentsController> logger, PaymentGateway paymentGateway, PaymentRepository paymentRepository)
        {
            _logger = logger;
            _paymentGateway = paymentGateway;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Make a payment
        /// </summary>
        /// <param name="paymentRequestDto">paymentRequest</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/payments
        ///         {
        ///             "Card": {
        ///                 "CardNumber": "4242424242424242",
        ///                 "Cvv": "100",
        ///                 "ExpirationDate": "1212"
        ///             },
        ///             "MerchantId": "FancyShop",
        ///             "AmountToCharge": {
        ///                 "Amount": 1000,
        ///                 "Currency": "EUR"
        ///             },
        ///             "MerchantReference": "eazeazea4d5q4s"
        ///         }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PaymentResponseDto))]
        [ProducesResponseType(400, Type = typeof(ValidationErrorDto))]
        public async Task<IActionResult> Post([FromBody][Required] PaymentRequestDto paymentRequestDto)
        {

            var command = DtoConverter.CreateCommand(paymentRequestDto);
            var paymentResponse = await _paymentGateway.ProcessPaymentRequestAsync(command);

            return paymentResponse.Match<IActionResult>(
                vr => new JsonResult(DtoConverter.ToDto(vr))
                {
                    StatusCode = 400
                },r => Created(@"/api/payments",new PaymentResponseDto
                {
                    PaymentId = r.PaymentId,
                    PaymentStatus = r.PaymentStatus.ToString()
                }));
        }

       

        [HttpGet]
        public async Task<IActionResult> Get(string paymentId)
        {
            //TODO get merchant Id from http HEAD.M
            var payment = await _paymentRepository.GetAsync("", paymentId);
            return Ok(payment);
        }
    }
}