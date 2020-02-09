using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.WebApi.Controllers.Dto;

namespace PaymentChallenge.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentGateway _paymentGateway;
        private readonly PaymentRepository _paymentRepository;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public PaymentsController(PaymentGateway paymentGateway, 
            PaymentRepository paymentRepository,
            ClaimsPrincipal claimsPrincipal)
        {
            _paymentGateway = paymentGateway;
            _paymentRepository = paymentRepository;
            _claimsPrincipal = claimsPrincipal;
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

            var command = DtoConverter.CreateCommand(paymentRequestDto, _claimsPrincipal.Identity.Name);
            var paymentResponse = await _paymentGateway.ProcessPaymentRequestAsync(command);
            return  await paymentResponse.Match<IActionResult>(
                result => new JsonResult(DtoConverter.ToDto(result)),
                pr => Created(@"/api/payments", new PaymentResponseDto
                {
                    PaymentId = pr.PaymentId,
                    PaymentStatus = pr.PaymentStatus.ToString()
                })
            );
        }

        /// <summary>
        /// Retrive a payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpGet("{paymentId}")]
        [ProducesResponseType(200, Type = typeof(PaymentDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string paymentId)
        {
            var payment = await _paymentRepository.GetAsync(_claimsPrincipal.Identity.Name, paymentId);
            return await payment.Match<IActionResult>(
                p => Ok(p.ToDto()),
                NotFound);
        }
        
        /// <summary>
        /// List all payments of the authenticated merchant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var payments = await _paymentRepository.GetPaymentsAsync(_claimsPrincipal.Identity.Name);
            return Ok(payments.Select(payment => payment.ToDto()).ToArray());
        }
    }
}