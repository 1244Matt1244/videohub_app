using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/stripe")]
    public class StripeController : ControllerBase
    {
        private readonly StripeService _stripeService;

        public StripeController(StripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-payment-intent")]
        [Authorize] // Samo prijavljeni korisnici mogu plaćati
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest req)
        {
            try
            {
                var intent = await _stripeService.CreatePaymentIntent(req.Amount, req.Currency);
                return Ok(new
                {
                    clientSecret = intent.ClientSecret
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class PaymentRequest
    {
        public long Amount { get; set; } = 1000; // 10.00 USD
        public string Currency { get; set; } = "usd";
    }
}
